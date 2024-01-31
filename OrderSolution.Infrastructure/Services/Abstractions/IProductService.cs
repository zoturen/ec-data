using OrderSolution.Infrastructure.Dtos;

namespace OrderSolution.Infrastructure.Services.Abstractions;

public interface IProductService
{
    Task<IEnumerable<ProductSimpleDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByArticleNumberAsync(string articleNumber);
    Task<ProductDto> CreateProductAsync(ProductCreateDto dto);
    Task<bool> UpdateProductAsync(string articleNumber, ProductCreateDto dto);
    Task<bool> DeleteProductAsync(string articleNumber);
}