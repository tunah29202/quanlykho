using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers.Auth;
using System.Threading.Tasks;
namespace Services.Core.Services
{
    public class OrderServices : BaseServices, IOrderServices
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<OrderDetail> orderDetailRepository;
        private readonly IRepository<OrderWarehouse> orderWarehouseRepository;
        private readonly IRepository<Invoice> invoiceRepository;
        public OrderServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            orderRepository = _unitOfWork.GetRepository<Order>();
            orderDetailRepository = _unitOfWork.GetRepository<OrderDetail>();
            orderWarehouseRepository = _unitOfWork.GetRepository<OrderWarehouse>();
            productRepository = _unitOfWork.GetRepository<Product>();
            invoiceRepository = _unitOfWork.GetRepository<Invoice>();
        }

        public async Task<PagedList<OrderResponse>> GetAll(OrderPagedRequest request)
        {
            PagedList<Order> Orders;
            if(request.get_all)
            {
                Orders = orderRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .Include(x => x.customer)
                                    .Include(x => x.order_details.Where(y => !y.del_flg))
                                        .ThenInclude(od => od.product)
                                            .ThenInclude(p => p.category)
                            .Include(x => x.payment_method)
                            .ToAllPageList();
            }
            else
            {
                Orders = await orderRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => x.order_warehouses.Any(ow => ow.warehouse_id == request.warehouse_id))
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.order_no.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .Include(x => x.customer)
                                    .Include(x => x.order_details.Where(y => !y.del_flg))
                                        .ThenInclude(od => od.product)
                                            .ThenInclude(p => p.category)
                                    .Include(x => x.payment_method)
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<OrderResponse>>(Orders);
            return dataMapping;
        }
        public async Task<PagedList<OrderResponse>> GetNotInInvoice(OrderPagedRequest request)
        {
            PagedList<Order> Orders;
            Orders = await orderRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .Where(x => x.order_warehouses.Any(ow => ow.warehouse_id == request.warehouse_id))
                            .Where(x => !string.IsNullOrEmpty(request.search) ? x.order_no.ToLower().Contains(request.search.ToLower()) : true)
                            .SortBy(request.sort ?? "updated_at.desc")
                            .Include(x => x.invoices)
                            .Where(x => x.invoices == null || x.invoices.All(inv => inv.del_flg == true))
                            .Include(x => x.customer)
                            .Include(x => x.order_details.Where(y => !y.del_flg))
                                 .ThenInclude(od => od.product)
                                 .ThenInclude(p => p.category)
                            .Include(x => x.payment_method)
                            .ToPagedListAsync(request.page, request.size);
            var dataMapping = _mapper.Map<PagedList<OrderResponse>>(Orders);
            return dataMapping;
        }


        public async Task<OrderResponse> GetById(Guid id)
        {
            var Order = await orderRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .Include(x => x.customer)
                                    .Include(x => x.order_details.Where(y => !y.del_flg))
                                        .ThenInclude(od => od.product)
                                            .ThenInclude(p => p.category)
                                    .Include(x => x.payment_method)
                                    .Include(x => x.order_warehouses!.Where(ow => ow.warehouse != null && !ow.del_flg))
                                        .ThenInclude(ow => ow.warehouse).FirstOrDefaultAsync();
            var data = _mapper.Map<OrderResponse>(Order);
            return data;
        }

        public async Task<int> Create(OrderRequest request)
        {
            var order = _mapper.Map<Order>(request);
            await orderRepository.AddAsync(order);
            if (request.warehouse_ids != null)
            {
                foreach (var warehouse_id in request.warehouse_ids)
                {
                    await orderWarehouseRepository.AddAsync(new OrderWarehouse()
                    {
                        order_id = order.id,
                        warehouse_id = warehouse_id,
                    });
                }
            }
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }   

        public async Task<int> Update(Guid id, OrderRequest request)
        {
            var oldOrder = _unitOfWork
                                .GetRepository<Order>()
                                .GetQuery()
                                .FindActiveById(id)
                                .Include(o => o.order_details.Where(od => !od.del_flg))
                                .AsNoTracking()
                                .FirstOrDefault();
            if(oldOrder == null)
                return -1;
            Guid[] productIds = request.order_details.Select(x => x.product_id).ToArray();
            Guid[] productIdsOld = oldOrder.order_details.Select(x => x.product_id).ToArray();
            
            Guid[] productIdsRemove = productIdsOld
                                        .Where(x => !productIds.Contains(x))
                                        .ToArray();
            Guid[] productIdsUpdate = productIds
                                        .Where(x => productIdsOld.Contains(x))
                                        .ToArray();
            Guid[] productIdsAdd = productIds
                                    .Where(x => !productIdsOld.Contains(x))
                                    .ToArray();
            List<OrderDetail> order_details = _unitOfWork
                                .GetRepository<OrderDetail>()
                                .GetQuery()
                                .Where(x => x.order_id == id)
                                .ToList();
            //update or delete old order detail
            foreach (var detail in order_details)
            {
                if(productIdsRemove.Contains(detail.product_id))
                {
                    await orderDetailRepository.DeleteAsync(detail);
                }
                else if (productIdsUpdate.Contains(detail.product_id))
                {
                    OrderDetailRequest? order_detail = request.order_details
                                                            .Where(x => x.product_id.Equals(detail.product_id))
                                                            .FirstOrDefault();
                    if(order_detail != null)
                    {
                        _mapper.Map(order_detail, detail);
                    }
                }
            }
            //add new orderDetail
            foreach(var detailRequest in request.order_details)
            {
                var orderDetail = new OrderDetail();
                _mapper.Map(detailRequest, orderDetail);
                if(productIdsAdd.Contains(detailRequest.product_id))
                {
                    await orderDetailRepository.AddAsync(orderDetail);
                }
            }
            Order order = new Order();
            _mapper.Map(request, order);
            order.id = id;
            await orderRepository.UpdateAsync(order);
            var oldWarehouses = orderWarehouseRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => x.order_id == id)
                                .ToList();
            //delete old warehouse
            foreach (var oldWarehouse in oldWarehouses)
            {
                await orderWarehouseRepository.DeleteAsync(oldWarehouse);
            }
            if (request.warehouse_ids != null && request.warehouse_ids.Count > 0)
            {
                foreach (var orderWarehouse in request.warehouse_ids)
                {
                    var oldWarehouse = await orderWarehouseRepository
                                                .GetQuery()
                                                .FirstOrDefaultAsync(ow => ow.warehouse_id == orderWarehouse && ow.del_flg);
                    if (oldWarehouse != null)
                    {
                        oldWarehouse.order_id = id;
                        oldWarehouse.del_flg = false;
                    }
                    else
                    {
                        await orderWarehouseRepository.AddAsync(new OrderWarehouse()
                        {
                            order_id = id,
                            warehouse_id = orderWarehouse
                        });
                    }
                }
            }
            int count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Order = await orderRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .Include(x=>x.invoices.Where(i => i.del_flg == false))
                                    .FirstOrDefaultAsync();
            if(Order == null)
            {
                return -1;
            }
            if (Order.invoices != null && Order.invoices.Count() > 0)
            {
                return -2;
            }
            await orderRepository.DeleteAsync(Order);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
        public async Task<string> GetOrderNo(string code)
        {
            var order = await orderRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .Where(x => x.order_no.Contains(code))
                            .OrderBy(x => x.order_no)
                            .LastOrDefaultAsync();
            var order_no = code;
            if (order != null)
            {
                var nChar = order.order_no[order.order_no.Length - 1];
                int nInt = int.Parse(nChar.ToString()) + 1;
                order_no += nInt;
            }
            else
            {
                order_no += 1;
            }
            return order_no;
        }
        public async Task<StatisticalResponse> GetStatistical(DateTime startDate, DateTime endDate, Guid? warehouse_id)
        {
            var startDateOnly = startDate.Date;
            var endDateOnly = endDate.Date;

            var orders = await orderRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(i => i.order_date.Date >= startDateOnly && i.order_date.Date <= endDateOnly )
                                .ToListAsync();
            Dictionary<DateTime, int> invoiceCountByDate = new Dictionary<DateTime, int>();
            foreach (var order in orders)
            {
                DateTime dateOnly = order.created_at.Date;
                if (invoiceCountByDate.ContainsKey(dateOnly))
                {
                    invoiceCountByDate[dateOnly]++;
                }
                else
                {
                    invoiceCountByDate[dateOnly] = 1;
                }
            }
            var sortedInvoiceCountByDate = invoiceCountByDate.OrderBy(x => x.Key);
            var labels = sortedInvoiceCountByDate.Select(x => x.Key.ToString("dd-MM")).ToArray();
            var dataSets = sortedInvoiceCountByDate.Select(x => x.Value).ToArray();

            var statisticalData = new StatisticalResponse
            {
                labels = labels,
                datasets = dataSets
            };
            return statisticalData;
        }
    }
}
