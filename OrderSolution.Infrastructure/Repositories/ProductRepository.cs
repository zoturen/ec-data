using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class ProductRepository(EcDbFirstContext context) : Repository<Product>(context), IProductRepository
{
    public override async Task<Product?> GetAsync(Expression<Func<Product, bool>> predicate)
    {
        try
        {
            return await context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Productdetail)
                .FirstOrDefaultAsync(predicate);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }

        return null!;
    }
    
    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            return await context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }

        return new List<Product>();
    }
}