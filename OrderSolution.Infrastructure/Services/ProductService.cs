using System.Diagnostics;
using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Services;

public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository) : IProductService
{
    public async Task<IEnumerable<ProductSimpleDto>> GetAllProductsAsync()
    {
        try
        {
            var products = await productRepository.GetAllAsync();
            return products.Select(p => p.ToSimpleDto());
        }
        catch (Exception e)
        {
           Debug.WriteLine($"ERROR: {e.Message}");
        }

        return new List<ProductSimpleDto>();
    }
    
    public async Task<ProductDto?> GetProductByIdAsync(string articleNumber)
    {
        try
        {
            var product = await productRepository.GetAsync(p => p.Articlenumber == articleNumber);
            return product?.ToDto();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }

        return null;
    }

    public async Task<ProductDto> CreateProductAsync(ProductCreateDto dto)
    {
        try
        {
            var categoryExists = await categoryRepository.ExistsAsync(x => x.Id == dto.CategoryId);
            if (!categoryExists)
                return null!;

            var productEntity = dto.ToEntity();
            var product = await productRepository.AddAsync(productEntity);
            return product.ToDto();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }
        return null!;
    }
    
    public async Task<bool> UpdateProductAsync(string articleNumber, ProductCreateDto dto)
    {
        try
        {
            var categoryExists = await categoryRepository.ExistsAsync(x => x.Id == dto.CategoryId);
            if (!categoryExists)
                return false;

            var productEntity = dto.ToEntity();
            await productRepository.UpdateAsync(p => p.Articlenumber == articleNumber, productEntity);
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }
        return false;
    }
    
    public async Task<bool> DeleteProductAsync(string articleNumber)
    {
        return await productRepository.DeleteAsync(p => p.Articlenumber == articleNumber);
    }
}