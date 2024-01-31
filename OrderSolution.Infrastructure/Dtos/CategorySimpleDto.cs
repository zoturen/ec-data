using OrderSolution.Infrastructure.Entities.Dbf;

namespace OrderSolution.Infrastructure.Dtos;

public record CategorySimpleDto(Guid Id, string Name);

public static partial class DtoExtensions
{
    public static CategorySimpleDto ToSimpleDto(this Category category)
    {
        return new CategorySimpleDto(Guid.Parse(category.Id), category.Name);
    }
}