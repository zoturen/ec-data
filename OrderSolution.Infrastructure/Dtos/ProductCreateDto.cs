using OrderSolution.Infrastructure.Entities.Dbf;

namespace OrderSolution.Infrastructure.Dtos;

public class ProductCreateDto
{
    public string ArticleNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string CategoryId { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Size { get; set; } = null!;
    public List<string> ImageUrl { get; set; } = [];
}

public static partial class DtoExtensions
{
    public static Product ToEntity(this ProductCreateDto dto)
    {
        return new Product
        {
            Articlenumber = dto.ArticleNumber,
            Name = dto.Name,
            Categoryid = dto.CategoryId,
            Price = dto.Price,
            Description = dto.Description,
            Stock = dto.Stock,
            Images = new List<Image>(dto.ImageUrl.Select(x => new Image {Url = x})),
            Productdetail = new Productdetail
            {
                Color = dto.Color,
                Size = dto.Size,
            }
        };
    }
}