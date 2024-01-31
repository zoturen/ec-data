using OrderSolution.Infrastructure.Entities.Dbf;

namespace OrderSolution.Infrastructure.Dtos;

public record ProductDto(string ArticleNumber, string Name, string Description, decimal Price, string Category, int Stock, List<ImageDto> Images, ProductDetailDto Details);

public static partial class DtoExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Articlenumber,
            product.Name,
            product.Description,
            product.Price,
            product.Category.Name,
            product.Stock,
            product.Images.Select(i => new ImageDto(i.Url)).ToList(),
            new ProductDetailDto(product.Productdetail?.Size, product.Productdetail?.Color)
        );
    }
}