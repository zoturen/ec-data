using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Repositories;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddDbContext<CfContext>(x =>
        {
            x.UseNpgsql(builder.Configuration["postgres:cfConnectionString"]);
        });
        
       services.AddDbContext<EcDbFirstContext>(x =>
        {
            x.UseNpgsql(builder.Configuration["postgres:dfConnectionString"]);
        });

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
        services.AddScoped<ICustomerDetailRepository, CustomerDetailRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
    }
    
    public static void UseInfrastructure(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<CfContext>();

        dbContext.Database.Migrate();
    }
}