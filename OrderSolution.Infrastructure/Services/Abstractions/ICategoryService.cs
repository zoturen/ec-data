using OrderSolution.Infrastructure.Dtos;

namespace OrderSolution.Infrastructure.Services.Abstractions;

public interface ICategoryService
{
    Task<IEnumerable<CategorySimpleDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryAsync(Guid id);
    Task<CategorySimpleDto> CreateCategoryAsync(CategoryCreateDto dto);
    Task<bool> UpdateCategoryAsync(Guid id, CategoryCreateDto dto);
    Task<bool> DeleteCategoryAsync(Guid id);
}