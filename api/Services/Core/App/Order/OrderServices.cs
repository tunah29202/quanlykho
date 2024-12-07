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
    public class OrderServices : BaseServices, IOrderServices
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<OrderDetail> orderDetailRepository;
        public OrderServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            orderRepository = _unitOfWork.GetRepository<Order>();
            orderDetailRepository = _unitOfWork.GetRepository<OrderDetail>();
            productRepository = _unitOfWork.GetRepository<Product>();
        }

        public async Task<PagedList<OrderResponse>> GetAll(PagedRequest request)
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
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<OrderResponse>(Order);
            return data;
        }

        public async Task<int> Create(OrderRequest request)
        {
            var order = _mapper.Map<Order>(request);
            await orderRepository.AddAsync(order);
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
            int count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Order = await orderRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if(Order == null)
            {
                return -1;
            }
            await orderRepository.DeleteAsync(Order);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
