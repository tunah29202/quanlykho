using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers.Auth;
namespace Services.Core.Services
{
    public class CartonServices : BaseServices, ICartonServices
    {
        private readonly IRepository<Carton> cartonRepository;
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<CartonDetail> cartonDetailRepository;
        public CartonServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            cartonRepository = _unitOfWork.GetRepository<Carton>();
            cartonDetailRepository = _unitOfWork.GetRepository<CartonDetail>();
            productRepository = _unitOfWork.GetRepository<Product>();
        }

        public async Task<PagedList<CartonResponse>> GetAll(PagedRequest request)
        {
            PagedList<Carton> Cartons;
            if(request.get_all)
            {
                Cartons = cartonRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Cartons = await cartonRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.carton_no.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .Include(x => x.carton_details.Where(p => !p.del_flg))
                                    .ThenInclude(x => x.product).ThenInclude(pr => pr.category)
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<CartonResponse>>(Cartons);
            return dataMapping;
        }

        public async Task<CartonResponse> GetById(Guid id)
        {
            var Carton = await cartonRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .Include(x => x.carton_details.Where(y => !y.del_flg))
                                        .ThenInclude(cd => cd.product)
                                            .ThenInclude(p => p.category)
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<CartonResponse>(Carton);
            return data;
        }

        public async Task<int> Create(CartonRequest request)
        {
            var Carton = _mapper.Map<Carton>(request);
            await cartonRepository.AddAsync(Carton);
            Guid[] productIds = request.carton_details.Select(x => x.product_id).ToArray();
            var products = await _unitOfWork
                                    .GetRepository<Product>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterByIds(productIds)
                                    .ToListAsync();
            foreach (var product in products)
            {
                product.quantity = product.quantity - (int)request.carton_details
                                                                    .Where(x => x.product_id == product.id)
                                                                    .Select(x=>x.quantity)
                                                                    .FirstOrDefault();
                await productRepository.UpdateAsync(product);
            }
            
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }   

        public async Task<int> Update(Guid id, CartonRequest request)
        {
            var count = -1;
            Carton carton;
            List<CartonDetail> carton_details;
            var oldCarton = _unitOfWork
                                    .GetRepository<Carton>()
                                    .GetQuery()
                                    .FindActiveById(id)
                                    .Include(x=>x.carton_details.Where(p=>!p.del_flg))
                                    .AsNoTracking()
                                    .FirstOrDefault();
            if(oldCarton == null)
            {
                return count;
            }

            //update ton kho product
            Guid[] productIds = request.carton_details.Select(x => x.product_id).ToArray();
            Guid[] productIdsOld = oldCarton.carton_details.Select(x => x.product_id).ToArray();
            
            Guid[] productIdsRemove = productIdsOld
                                        .Where(x => !productIds.Contains(x))
                                        .ToArray();
            Guid[] productIdsUpdate = productIds
                                        .Where(x => productIdsOld.Contains(x))
                                        .ToArray();
            Guid[] productIdsAdd = productIds
                                    .Where(x => !productIdsOld.Contains(x))
                                    .ToArray();
            var products = await _unitOfWork
                                    .GetRepository<Product>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterByIds(productIds.Union(productIdsOld).ToArray())
                                    .ToListAsync();
            foreach(var product in products)
            {
                int quantityNew = request.carton_details
                                            .Where(x => x.product_id == product.id)
                                            .Select(x => x.quantity)
                                            .FirstOrDefault();
                int quantityOld = (int)oldCarton.carton_details
                                        .Where(x => x.product_id == product.id)
                                        .Select(x => x.quantity)
                                        .FirstOrDefault();
                if (productIdsRemove.Contains(product.id))  //delete product from carton
                {
                    product.quantity += quantityOld;
                }                           
                else if (productIdsAdd.Contains(product.id)) //add new product to carton
                {
                    product.quantity -= quantityNew;
                }
                else if (productIdsUpdate.Contains(product.id)) //update product in carton
                {
                    product.quantity -= quantityNew - quantityOld;
                }
                await productRepository.UpdateAsync(product);
            }


            //update carton
            carton = new Carton();
            _mapper.Map(request, carton);
            carton.id = id;
            await cartonRepository.UpdateAsync(carton);

            //update cartonDetail
            carton_details = _unitOfWork
                                .GetRepository<CartonDetail>()
                                .GetQuery()
                                .Where(x => x.carton_id == id)
                                .ToList();
            //update records
            foreach (var detail in carton_details)
            {
                if(productIdsRemove.Contains(detail.product_id))
                {
                    await cartonDetailRepository.DeleteAsync(detail);
                }
                else if (productIdsUpdate.Contains(detail.product_id))
                {
                    CartonDetailRequest? carton_detail = request.carton_details
                                                            .Where(x => x.product_id.Equals(detail.product_id))
                                                            .FirstOrDefault();
                    if(carton_detail != null)
                    {
                        _mapper.Map(carton_detail, detail);
                    }
                }
            }
            //add records
            foreach(var detail in carton_details)
            {
                CartonDetail cartonDetail = new CartonDetail();
                _mapper.Map(detail, cartonDetail);
                if(productIdsAdd.Contains(detail.product_id))
                {
                    await cartonDetailRepository.AddAsync(detail);
                }
            }
            count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Carton = await cartonRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .Include(x => x.invoices.Where(i => i.del_flg == false))
                                    .Include(x => x.carton_details.Where(cd => cd.del_flg == false))
                                    .FirstOrDefaultAsync();
            if(Carton == null)
            {
                return -1;
            }
            if(Carton.invoices != null && Carton.invoices.Count() > 0)
            {
                return -2;
            }
            //update ton kho
            Guid[] productIds = Carton.carton_details.Select(x => x.product_id).ToArray();
            var products = await _unitOfWork
                                    .GetRepository<Product>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterByIds(productIds)
                                    .ToListAsync();
            foreach (var product in products)
            {
                product.quantity += (int)Carton.carton_details
                                            .Where(cd => cd.product_id == product.id)
                                            .Select(p => p.quantity)
                                            .FirstOrDefault();
                await productRepository.UpdateAsync(product); 
            }
            await cartonRepository.DeleteAsync(Carton);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
