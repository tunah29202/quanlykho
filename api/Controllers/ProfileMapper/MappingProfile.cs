using AutoMapper;
using Services.Core.Contracts;
using Database.Entities;
using System.Globalization;
using Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRequest, User>();
        CreateMap<CustomerRegisterRequest, User>();
        CreateMap<PagedList<User>, PagedList<UserResponse>>();
        CreateMap<User, UserResponse>()
         .ForMember(dest => dest.role_cd, opt => opt.MapFrom(src => src.user_role.role.code))
         .ForMember(dest => dest.role_name, opt => opt.MapFrom(src => src.user_role.role.name))
         .ForMember(dest => dest.warehouse_ids, opt => opt.MapFrom(src =>
                 src.user_warehouses != null
                 ? src.user_warehouses.Select(uw => uw.warehouse != null ? uw.warehouse.id : Guid.Empty).ToList()
                 : new List<Guid>()
         ))
         .ForMember(dest => dest.warehouse_names, opt => opt.MapFrom(src =>
                 src.user_warehouses != null
                 ? src.user_warehouses.Select(uw => uw.warehouse != null ? uw.warehouse.name : string.Empty).ToList()
                 : new List<string>()
         )); 
        CreateMap<ResourceRequest, Resource>();
        CreateMap<PagedList<Resource>, PagedList<ResourceResponse>>();
        CreateMap<Resource, ResourceResponse>();

        CreateMap<RoleRequest, Role>()
               .ForMember(dest => dest.permissions, opt => opt.Ignore());
        CreateMap<PagedList<Role>, PagedList<RoleResponse>>();
        CreateMap<Role, RoleResponse>()
            .ForMember(dest => dest.permissions, opt => opt.MapFrom(src => src.permissions.Select(x => x.function.code).ToList()));

        CreateMap<FunctionRequest, Function>();
        CreateMap<PagedList<Function>, PagedList<FunctionResponse>>();
        CreateMap<Function, FunctionResponse>();

        CreateMap<WarehouseRequest, Warehouse>();
        CreateMap<PagedList<Warehouse>, PagedList<WarehouseResponse>>();
        CreateMap<Warehouse, WarehouseResponse>();

        CreateMap<ShipperRequest, Shipper>();
        CreateMap<PagedList<Shipper>, PagedList<ShipperResponse>>();
        CreateMap<Shipper, ShipperResponse>();

        CreateMap<InvoiceRequest, Invoice>()
                .ForMember(dest => dest.invoice_date, opt => opt.MapFrom(src => DateTime.ParseExact(src.invoice_date, "M/d/yyyy, h:mm:ss tt", CultureInfo.GetCultureInfo("en-En"))))
                .ForMember(dest => dest.shipped_date, opt => opt.MapFrom(src => DateTime.ParseExact(src.shipped_date, "M/d/yyyy, h:mm:ss tt", CultureInfo.GetCultureInfo("en-En"))));
        CreateMap<PagedList<Invoice>, PagedList<InvoiceResponse>>();
        CreateMap<Invoice, InvoiceResponse>();
        
        CreateMap<ProductRequest, Product>();
        CreateMap<ProductExportRequest, Product>();
        CreateMap<PagedList<Product>, PagedList<ProductResponse>>();
        CreateMap<Product, ProductResponse>();
        
        CreateMap<CartonRequest, Carton>();
        CreateMap<CartonDetailRequest, CartonDetail>();
        CreateMap<PagedList<Carton>, PagedList<CartonResponse>>();
        CreateMap<Carton, CartonResponse>();

        CreateMap<OrderRequest, Order>()
                 .ForMember(dest => dest.order_date, opt => opt.MapFrom(src => DateTime.ParseExact(src.order_date, "M/d/yyyy, h:mm:ss tt", CultureInfo.GetCultureInfo("en-En"))));
        CreateMap<OrderDetailRequest, OrderDetail>();
        CreateMap<PagedList<Order>, PagedList<OrderResponse>>();
        CreateMap<Order, OrderResponse>()
                 .ForMember(dest => dest.warehouse_ids, opt => opt.MapFrom(src =>
                         src.order_warehouses != null
                         ? src.order_warehouses.Select(ow => ow.warehouse != null ? ow.warehouse.id : Guid.Empty).ToList()
                         : new List<Guid>()
                 ))
                 .ForMember(dest => dest.warehouse_names, opt => opt.MapFrom(src =>
                         src.order_warehouses != null
                         ? src.order_warehouses.Select(ow => ow.warehouse != null ? ow.warehouse.name : string.Empty).ToList()
                         : new List<string>()
                 ));

        CreateMap<CategoryRequest, Category>()
            .ForMember(dest => dest.ingredients, opt => opt.Ignore());
        CreateMap<PagedList<Category>, PagedList<CategoryResponse>>();
        CreateMap<Category, CategoryResponse>();
        
        CreateMap<CustomerRequest, Customer>();
        CreateMap<PagedList<Customer>, PagedList<CustomerResponse>>();
        CreateMap<Customer, CustomerResponse>();

        CreateMap<IngredientRequest, Ingredient>();
        CreateMap<PagedList<Ingredient>, PagedList<IngredientResponse>>();
        CreateMap<Ingredient, IngredientResponse>();

        CreateMap<PaymentMethodRequest, PaymentMethod>();
        CreateMap<PagedList<PaymentMethod>, PagedList<PaymentMethodResponse>>();
        CreateMap<PaymentMethod, PaymentMethodResponse>();

    }
}