using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderSolution.Api;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class CategoryRepository(EcDbFirstContext context) : Repository<Category>(context), ICategoryRepository
{
    public override Task<Category?> GetAsync(Expression<Func<Category, bool>> predicate)
    {
        try
        {
            return context.Categories
                .Include(p => p.Products)
                .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(predicate);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }

        return null!;
    }
}