using OrderSolution.Infrastructure.Entities.Dbf;

namespace OrderSolution.Infrastructure.Dtos;

public record ProductSimpleDto(string ArticleNumber, string Name, string Description, decimal Price, int Stock, string Category, string ImageUrl);

public static partial class DtoExtensions
{
    public static ProductSimpleDto ToSimpleDto(this Product product)
    {
        return new ProductSimpleDto(product.Articlenumber, product.Name, product.Description, product.Price, product.Stock, product.Category.Name, product.Images.FirstOrDefault()?.Url ?? null!);
    }
}