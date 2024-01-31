using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var productEndpoints = endpoints.MapGroup("/products")
            .WithTags("Products API");
        
        productEndpoints.MapPost("/", async (ProductCreateDto productCreateDto, IProductService productService) =>
        {
            var product = await productService.CreateProductAsync(productCreateDto);
            if (product == null!)
                return Results.BadRequest();
            return Results.Created($"/products/{product.ArticleNumber}", product);
        });
        
        productEndpoints.MapGet("/", async (IProductService productService) =>
        {
            var products = await productService.GetAllProductsAsync();
            return Results.Ok(products);
        });
        
        productEndpoints.MapGet("/{articleNumber}", async (string articleNumber, IProductService productService) =>
        {
            var product = await productService.GetProductByArticleNumberAsync(articleNumber);
            if (product == null!)
                return Results.NotFound();
            return Results.Ok(product);
        });
        
        productEndpoints.MapPut("/{articleNumber}", async (string articleNumber, ProductCreateDto productCreateDto, IProductService productService) =>
        {
            var result = await productService.UpdateProductAsync(articleNumber, productCreateDto);
            return result ? Results.NotFound() : Results.Ok();
        });
        
        productEndpoints.MapDelete("/{articleNumber}", async (string articleNumber, IProductService productService) =>
        {
            var result = await productService.DeleteProductAsync(articleNumber);
            return result ? Results.NotFound() : Results.Ok();
        });
    }
}