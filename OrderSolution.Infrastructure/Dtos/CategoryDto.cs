using OrderSolution.Infrastructure.Entities.Dbf;

namespace OrderSolution.Infrastructure.Dtos;

public record CategoryDto(Guid Id, string Name, List<ProductSimpleDto> Products);

public static partial class DtoExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto(
            Guid.Parse(category.Id),
            category.Name,
            category.Products.Select(x => x.ToSimpleDto()).ToList()
        );
    }
}