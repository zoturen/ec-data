using System.Diagnostics;
using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<IEnumerable<CategorySimpleDto>> GetCategoriesAsync()
    {
        try
        {
            var categories = await categoryRepository.GetAllAsync();
            return categories.Select(c => c.ToSimpleDto());
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }
        return new List<CategorySimpleDto>();
    }
    
    public async Task<CategoryDto?> GetCategoryAsync(Guid id)
    {
        try
        {
            var category = await categoryRepository.GetAsync(c => c.Id == id.ToString());
            return category?.ToDto();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }
        return null;
    }
    
    public async Task<CategorySimpleDto> CreateCategoryAsync(CategoryCreateDto dto)
    {
        try
        {
            var categoryEntity = await categoryRepository.AddAsync(new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name
            });
            return categoryEntity?.ToSimpleDto()!;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }
        return null!;
    }
    
    public async Task<bool> UpdateCategoryAsync(Guid id, CategoryCreateDto dto)
    {
        try
        {
            var result = await categoryRepository.UpdateAsync(c => c.Id == id.ToString(), new Category
            {
                Name = dto.Name
            });
            return result;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }
        return false;
    }
    
    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
            var result = await categoryRepository.DeleteAsync(c => c.Id == id.ToString());
            return result;
    }
}