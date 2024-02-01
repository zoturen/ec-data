using OrderSolution.Infrastructure.Dtos;

namespace OrderSolution.Infrastructure.Services.Abstractions;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(OrderCreateDto dto);
    Task<OrderDto?> GetOrderAsync(Guid orderId);
    Task<IEnumerable<OrderDto>> GetOrdersAsync();
    Task<IEnumerable<OrderDto>> GetOrdersAsync(Guid customerId);
    Task<bool> PayOrderAsync(Guid orderId);
    Task<bool> DeleteOrderAsync(Guid orderId);
}