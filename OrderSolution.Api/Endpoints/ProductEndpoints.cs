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
        
        productEndpoints.MapPut("/{articleNumber}", async (string articleNumber, ProductUpdateDto productUpdateDto, IProductService productService) =>
        {
            var result = await productService.UpdateProductAsync(articleNumber, productUpdateDto);
            return result ? Results.Ok() : Results.NotFound();
        });
        
        productEndpoints.MapDelete("/{articleNumber}", async (string articleNumber, IProductService productService) =>
        {
            var result = await productService.DeleteProductAsync(articleNumber);
            return result ? Results.Ok() : Results.NotFound();
        });
        
        productEndpoints.MapPost("/{articleNumber}/images", async (string articleNumber, ProductImageCreateDto productImageCreateDto, IProductService productService) =>
        {
            var product = await productService.AddImageToProductAsync(articleNumber, productImageCreateDto);
            return product == null! ? Results.BadRequest() : Results.Created($"/products/{product.ArticleNumber}", product);
        });
        
        productEndpoints.MapDelete("/{articleNumber}/images/{imageId:guid}", async (string articleNumber, Guid imageId, IProductService productService) =>
        {
            var result = await productService.RemoveImageFromProductAsync(articleNumber, imageId);
            return result ? Results.Ok() : Results.BadRequest();
        });
        
        productEndpoints.MapPut("/{articleNumber}/images/{imageId:guid}", async (string articleNumber, Guid imageId, ImageUpdateDto imageUpdateDto, IImageService imageService) =>
        {
            var result = await imageService.UpdateImageAsync(imageId, imageUpdateDto);
            return result ? Results.Ok() : Results.BadRequest();
        });
    }
}