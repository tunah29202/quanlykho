using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers.Excel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;
using Microsoft.AspNetCore.Mvc;
namespace Services.Core.Services
{
    public class ProductServices : BaseServices, IProductServices
    {
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly ILocalizeServices ls;
        private readonly IWebHostEnvironment env;
        private readonly string _imageStoragePath = "Images";
        private readonly string Template = "Template";

        public ProductServices(IUnitOfWork _unitOfWork, IMapper _mapper, ILocalizeServices _ls, IWebHostEnvironment _env) : base(_unitOfWork, _mapper) 
        {
            ls = _ls; 
            env = _env;
            productRepository = _unitOfWork.GetRepository<Product>();
            categoryRepository = _unitOfWork.GetRepository<Category>();
        }

        public async Task<PagedList<ProductResponse>> GetAll(ProductPagedRequest request)
        {
            PagedList<Product> Products;
            if(request.get_all)
            {
                Products = productRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .Include(x => x.category)
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Products = await productRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .Include(x => x.category)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<ProductResponse>>(Products);
            return dataMapping;
        }

        public async Task<ProductResponse> GetById(Guid id)
        {
            var Product = await productRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .Include(x => x.category)
                                    .Include(x => x.warehouse)
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<ProductResponse>(Product);
            return data;
        }
        public async Task<int> Create(ProductRequest request)
        {
            if (await CheckDuplicateProductCode(request.code))
            {
                return -2;
            }
            var Product = _mapper.Map<Product>(request);
            if(request.image != null && request.image.Length != 0)
            {
                var directory = Path.Combine(env.WebRootPath, _imageStoragePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                };
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.image.FileName)}";
                string filePath = Path.Combine(directory, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.image.CopyToAsync(stream);
                }
                if(!File.Exists(filePath))
                {
                    return -3;
                }
                Product.image = fileName;
            }
            await productRepository.AddAsync(Product);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, ProductRequest request)
        {
            if (await CheckDuplicateProductCode(request.code, id))
            {
                return -2;
            }
            var Product = await _unitOfWork
                                    .GetRepository<Product>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if(Product == null)
            {
                return -1;
            }
            string image = Product.image?? "";
            _mapper.Map(request, Product);

            if (request.image != null && request.image.Length != 0)
            {
                if (string.IsNullOrEmpty(_imageStoragePath))
                    return 0;

                if (!Directory.Exists(_imageStoragePath))
                {
                    Directory.CreateDirectory(_imageStoragePath);
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.image.FileName)}";
                string filePath = Path.Combine(_imageStoragePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.image.CopyToAsync(stream);
                }
                if (!File.Exists(filePath))
                {
                    return -4;
                }
                Product.image = fileName;
            }
            else
                Product.image = image;
            await productRepository.UpdateAsync(Product);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
        public async Task<int> Delete(Guid id)
        {
            var Product = await productRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .Include(x => x.carton_details.Where(i => i.del_flg == false))
                                    .FirstOrDefaultAsync();
            if(Product == null)
            {
                return -1;
            }
            if (Product.carton_details != null && Product.carton_details.Count > 0)
                return -2;
            await productRepository.DeleteAsync(Product);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<(object?, MemoryStream?)> ImportExcel(ProductImportRequest request)
        {
            var directory = Path.Combine(env.WebRootPath, _imageStoragePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            };
            const int totalCol = 13;
            (List<ProductExportRequest> lstProduct, List<string> messages) = await ExcelImport.getDataFromExcel<ProductExportRequest>(request.file, totalCol, directory);
            Dictionary<int, List<string>> lstErrors = new Dictionary<int, List<string>>();
            ProductExportRequestValidator validator = new ProductExportRequestValidator();
            for(int i = 0; i < lstProduct.Count; i++)
            {
                lstProduct[i].warehouse_id = request.warehouse_id;
                ValidationResult results = validator.Validate(lstProduct[i]);
                if (!results.IsValid)
                {
                    foreach(var error in results.Errors)
                    {
                        ExcelImport.addError(lstErrors, i, error.ErrorMessage);
                    }
                }
                if (await CheckDuplicateProductCode(lstProduct[i].code))
                {
                    ExcelImport.addError(lstErrors, i, $"Product Code ({lstProduct[i].code}) đã tồn tại trong DB");
                    continue;
                }
                if (!string.IsNullOrEmpty(lstProduct[i].category_name))
                {
                    var category = await categoryRepository
                                                .GetQuery()
                                                .ExcludeSoftDeleted()
                                                .Include(y => y.ingredients.Where(y => y.del_flg == false))
                                                .FirstOrDefaultAsync(u => u.name == lstProduct[i].category_name);
                    if (category == null)
                        ExcelImport.addError(lstErrors, i, "Không tìm thấy danh mục hàng hóa đã chọn!");
                    else
                    {
                        lstProduct[i].category_id = category.id;

                        if (category.ingredients != null && 
                            !category.ingredients.Select(s => s.name).Contains(lstProduct[i].ingredient))
                            ExcelImport.addError(lstErrors, i, "Không tìm thấy nguyên liệu đã chọn!");
                    }
                }
            }
            if (messages.Count > 0)
            {
                return (new BaseResponse(ResponseCode.Invalid, ls.Get(Modules.Core, Screen.Product, MessageKey.I_DUPLICATE_003)), null);
            }
            if(lstErrors.Count > 0)
            {
                var result = await ExcelImport.getFileError(request.file, lstErrors, ExcelImport.ColumnIndexToLetter(totalCol+1));
                if (result.Item1 != null)
                    return (result.Item1, null);
                return(null, result.Item2);
            }
            foreach(var item in lstProduct)
            {
                await CreateNewProduct(item);
            };
            return (new BaseResponse(ResponseCode.Success, ls.Get(Modules.Core, Screen.Message, MessageKey.S_IMPORT)), null);
        }

        public async Task<int> CreateNewProduct(ProductExportRequest request)
        {
            var Product = _mapper.Map<Product>(request);
            await productRepository.AddAsync(Product);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        private async Task<bool> CheckDuplicateProductCode(string? code, Guid? id = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            if(id.HasValue && id != Guid.Empty)
            {
                var get_product = await _unitOfWork.GetRepository<Product>()
                                                    .GetQuery()
                                                    .ExcludeSoftDeleted()
                                                    .FirstOrDefaultAsync(x => x.id == id);
                if(get_product != null)
                {
                    if(get_product.code == code)
                    {
                        return false;
                    }
                }
            }
            var product = await _unitOfWork.GetRepository<Product>()
                                        .GetQuery()
                                        .ExcludeSoftDeleted()
                                        .Where(x => x.code == code)
                                        .ToListAsync();

            if (product.Count > 0)
            {
                return true;
            }
            return false;
        }

    }
}
