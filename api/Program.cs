using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Database;
using Services.Common.Repository;
using Services.Core;
using Services.Core.Contracts;
using FluentValidation.AspNetCore;
using Common;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); 
});
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddAuthorization();
builder.Services.AddCoreService(configuration);
builder.Services.Configure<AuthSetting>(configuration.GetSection("AuthSetting"));
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddFluentValidation(c =>
{
    c.DisableDataAnnotationsValidation = true;
    c.ImplicitlyValidateChildProperties = true;
    c.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});
builder.Services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();
builder.Services.AddCors(builder =>
{
    builder.AddPolicy(
        name: "AllowSpecificOrigins",
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        }
        );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();
app.UseStaticFiles();
app.Run();
