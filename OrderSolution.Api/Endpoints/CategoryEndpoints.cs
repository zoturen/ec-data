using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void AddCategoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", async (ICategoryService categoryService) =>
            Results.Ok(await categoryService.GetCategoriesAsync()));

        endpoints.MapGet("/{id:guid}", async (Guid id, ICategoryService categoryService) =>
        {
            var category = await categoryService.GetCategoryAsync(id);
            return category is null ? Results.NotFound() : Results.Ok(category);
        });
        
        endpoints.MapPost("/", async (CategoryCreateDto dto, ICategoryService categoryService) =>
        {
            var category = await categoryService.CreateCategoryAsync(dto);
            return Results.Created($"/{category.Id}", category);
        });
        
        endpoints.MapPut("/{id:guid}", async (Guid id, CategoryCreateDto dto, ICategoryService categoryService) =>
        {
            var result = await categoryService.UpdateCategoryAsync(id, dto);
            return result ? Results.Ok() : Results.NotFound();
        });
        
        endpoints.MapDelete("/{id:guid}", async (Guid id, ICategoryService categoryService) =>
        {
            var result = await categoryService.DeleteCategoryAsync(id);
            return result ? Results.Ok() : Results.NotFound();
        });
    }
}