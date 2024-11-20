using AutoMapper;
using Services.Core.Contracts;
using Database.Entities;
using Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRequest, User>();
        CreateMap<PagedList<User>, PagedList<UserResponse>>();
        CreateMap<User, UserResponse>();

        CreateMap<ResourceRequest, Resource>();
        CreateMap<PagedList<Resource>, PagedList<ResourceResponse>>();
        CreateMap<Resource, ResourceResponse>();

        CreateMap<RoleRequest, Role>();
        CreateMap<PagedList<Role>, PagedList<RoleResponse>>();
        CreateMap<Role, RoleResponse>();

        CreateMap<FunctionRequest, Function>();
        CreateMap<PagedList<Function>, PagedList<FunctionResponse>>();
        CreateMap<Function, FunctionResponse>();

        CreateMap<WarehouseRequest, Warehouse>();
        CreateMap<PagedList<Warehouse>, PagedList<WarehouseResponse>>();
        CreateMap<Warehouse, WarehouseResponse>();

        CreateMap<ShipperRequest, Shipper>();
        CreateMap<PagedList<Shipper>, PagedList<ShipperResponse>>();
        CreateMap<Shipper, ShipperResponse>();

        CreateMap<InvoiceRequest, Invoice>();
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

        CreateMap<OrderRequest, Order>();
        CreateMap<OrderDetailRequest, OrderDetail>();
        CreateMap<PagedList<Order>, PagedList<OrderResponse>>();
        CreateMap<Order, OrderResponse>();
        
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

        CreateMap<BankAccountRequest, BankAccount>();
        CreateMap<PagedList<BankAccount>, PagedList<BankAccountResponse>>();
        CreateMap<BankAccount, BankAccountResponse>();
        

    }
}