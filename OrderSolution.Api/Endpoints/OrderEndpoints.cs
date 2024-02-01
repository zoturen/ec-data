using Microsoft.AspNetCore.Mvc;
using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var orderEndpoints = endpoints.MapGroup("/orders")
            .WithTags("Orders API");
        
        orderEndpoints.MapPost("/", async (OrderCreateDto dto, IOrderService orderService) =>
        {
            var result = await orderService.CreateOrderAsync(dto);
            if (result == null!)
                return Results.BadRequest();
            return Results.Created($"/orders/{result.Id}", result);
        });
        
        orderEndpoints.MapGet("/", async ([FromQuery] Guid customerId, IOrderService orderService) =>
        {
            IEnumerable<OrderDto> orders;
            if (customerId == Guid.Empty)
            {
                orders = await orderService.GetOrdersAsync();
            }
            else
            {
                orders = await orderService.GetOrdersAsync(customerId);
            }
            return Results.Ok(orders);
            
        });
        
        orderEndpoints.MapGet("/{orderId:guid}", async (Guid orderId, IOrderService orderService) =>
        {
            var result = await orderService.GetOrderAsync(orderId);
            if (result == null!)
                return Results.NotFound();
            return Results.Ok(result);
        });
        
        orderEndpoints.MapPost("/{orderId:guid}/pay", async (Guid orderId, IOrderService orderService) =>
        {
            var result = await orderService.PayOrderAsync(orderId);
                if (result)
                    return Results.Ok();
                return Results.BadRequest();
        });
    }
}