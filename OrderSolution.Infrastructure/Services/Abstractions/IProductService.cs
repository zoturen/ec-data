using OrderSolution.Infrastructure.Dtos;

namespace OrderSolution.Infrastructure.Services.Abstractions;

public interface IProductService
{
    Task<IEnumerable<ProductSimpleDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByArticleNumberAsync(string articleNumber);
    Task<ProductDto> CreateProductAsync(ProductCreateDto dto);
    Task<bool> UpdateProductAsync(string articleNumber, ProductUpdateDto dto);
    Task<bool> DeleteProductAsync(string articleNumber);
    Task<ProductDto> AddImageToProductAsync(string articleNumber, ProductImageCreateDto dto);
    Task<bool> RemoveImageFromProductAsync(string articleNumber, Guid imageId);
    Task<bool> UpdateProductStockAsync(string articleNumber, int stock);
}