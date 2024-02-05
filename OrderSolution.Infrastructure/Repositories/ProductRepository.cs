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

    public override async Task<bool> UpdateAsync(Expression<Func<Product, bool>> predicate, Product entity)
    {
        try
        {
            var product = await context.Products
                .Include(p => p.Productdetail)
                .FirstOrDefaultAsync(predicate);
            if (product != null)
            {
                context.Entry(product).CurrentValues.SetValues(entity);

                
                if (product.Productdetail != null)
                {
                    product.Productdetail.Color = entity.Productdetail?.Color ?? null!;
                    product.Productdetail.Size = entity.Productdetail?.Size ?? null!;
                }
                
                await context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }
        return false;
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

    public async Task<Product> AddImageAsync(string articleNumber, Image image)
    {
        try
        {
            var product = await context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Articlenumber == articleNumber);

            if (product != null)
            {
                product.Images.Add(image);
                await context.SaveChangesAsync();
                return product;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }
        
        return null!;
    }

    public async Task<bool> RemoveImageAsync(string articleNumber, Guid imageId)
    {
        try
        {
            var product = await context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Articlenumber == articleNumber);

            var image = product?.Images.FirstOrDefault(i => i.Id == imageId.ToString());
            if (image != null)
            {
                if (product!.Images.Remove(image))
                {
                    await context.SaveChangesAsync();
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }

        return false;
    }

    public override async Task<bool> DeleteAsync(Expression<Func<Product, bool>> predicate)
    {
        var product = await context.Products
            .Include(p => p.Images)
            .Include(p => p.Productdetail)
            .FirstOrDefaultAsync(predicate);
        if (product != null)
        {
            if (product.Productdetail != null)
            {
                var productDetail = product.Productdetail;
                product.Productdetail = null;
                context.Productdetails.Remove(productDetail);
            }

            if (product.Images.Count != 0)
            {
                var images = product.Images.ToList();
                product.Images.Clear();
                context.Images.RemoveRange(images);
            }

            await context.SaveChangesAsync();
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}