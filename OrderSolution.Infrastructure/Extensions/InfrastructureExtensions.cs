using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderSolution.Infrastructure.Data;

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
    }
    
    public static void UseInfrastructure(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<CfContext>();

        dbContext.Database.Migrate();
    }
}