using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var customerEndpoint = endpoints.MapGroup("/customers")
            .WithTags("Customers API");
        
        customerEndpoint.MapPost("/", async (CustomerCreateDto customerCreateDto, ICustomerService customerService) =>
        {
            var customer = await customerService.CreateAsync(customerCreateDto);
            if (customer == null!)
                return Results.BadRequest();

            return Results.Created($"/{customer.Id}", customer);
        });
        
        customerEndpoint.MapGet("/", async (ICustomerService customerService) =>
        {
            var customers = await customerService.GetAllAsync();
            return Results.Ok(customers);
        });
        
        customerEndpoint.MapGet("/{id:guid}", async (Guid id, ICustomerService customerService) =>
        {
            var customer = await customerService.GetAsync(id);
            if (customer == null!)
                return Results.NotFound();

            return Results.Ok(customer);
        });
        
        customerEndpoint.MapPut("/{id:guid}", async (Guid id, CustomerCreateDto customerUpdateDto, ICustomerService customerService) =>
        {
            var result = await customerService.UpdateAsync(id, customerUpdateDto);
            if (result)
                return Results.NotFound();

            return Results.Ok();
        });
        
        customerEndpoint.MapDelete("/{id:guid}", async (Guid id, ICustomerService customerService) =>
        {
            var result = await customerService.DeleteAsync(id);
            if (result)
                return Results.NotFound();

            return Results.Ok();
        });
    }
}