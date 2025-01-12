using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Services.Core.Interfaces;
using Services.Core.Services;
using Services.Common.Repository;
using Common;
using Helpers.Cache;
using Helpers.Email;

namespace Services.Core
{
    public static class ServicesRegister
        {
            public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration configuration )
            {
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<ICurrentUserService, ApiServiceContext>();
                services.AddScoped<IUserServices, UserServices>();
                services.AddScoped<IRoleServices, RoleServices>();
                services.AddScoped<IPaymentMethodServices, PaymentMethodServices>();
                services.AddScoped<IFunctionServices, FunctionServices>();
                services.AddScoped<ICartonServices, CartonServices>();
                services.AddScoped<ICategoryServices, CategoryServices>();
                services.AddScoped<ICustomerServices, CustomerServices>();
                services.AddScoped<IOrderServices, OrderServices>();
                services.AddScoped<IIngredientServices, IngredientServices>();
                services.AddScoped<IInvoiceServices, InvoiceServices>();
                services.AddScoped<IProductServices, ProductServices>();
                services.AddScoped<IShipperServices, ShipperServices>();
                services.AddScoped<IWarehouseServices, WarehouseServices>();
                services.AddScoped<IAuthServices, AuthServices>();
                services.AddScoped<IResourceServices, ResourceServices>();
                services.AddScoped<ICacheServices, CacheServices>();
                services.AddScoped<ILocalizeServices, LocalizeServices>();
                services.AddScoped<ILogServices, LogServices>();
                services.AddScoped<IEmailHelpers, EmailHelpers>();

            return services;
            }
        }
    }