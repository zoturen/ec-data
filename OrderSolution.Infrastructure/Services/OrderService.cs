using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Services;

public class OrderService(IOrderRepository orderRepository, IProductService productService) : IOrderService
{
    public async Task<OrderDto> CreateOrderAsync(OrderCreateDto dto)
    {
        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            PaidAt = default,
            CreatedAt = DateTime.UtcNow,
            CustomerId = dto.CustomerId
        };
        
        var orderItems = new List<OrderItemEntity>();
        foreach (var orderItem in dto.OrderItems)
        {
            var product = await productService.GetProductByArticleNumberAsync(orderItem.ArticleNumber);
            if (product == null)
            {
                return null!; // should return a message to the user
            }

            if (product.Stock < orderItem.Quantity)
            {
                continue; // should return a message to the user and leave the item in basket
            }
            var orderItemEntity = new OrderItemEntity
            {
                OrderId = order.Id,
                ArticleNumber = product.ArticleNumber,
                Quantity = orderItem.Quantity,
                UnitPrice = product.Price,
                TotalAmount = product.Price * orderItem.Quantity
            };
            var productResult = await productService.UpdateProductStockAsync(product.ArticleNumber, product.Stock - orderItem.Quantity);
            if (!productResult)
            {
                return null!; // should return a message to the user and leave the item in basket
            }
            orderItems.Add(orderItemEntity);
        }
        order.OrderItems = orderItems;
        order.TotalAmount = orderItems.Sum(x => x.TotalAmount);
        
        var orderEntity = await orderRepository.AddAsync(order);
        if (orderEntity != null!)
        {
            return new OrderDto(order.Id, order.CustomerId, order.TotalAmount, order.PaidAt, order.CreatedAt, []);
        }

        return null!;
    }
    
    public async Task<OrderDto?> GetOrderAsync(Guid orderId)
    {
        var order = await orderRepository.GetAsync(x => x.Id == orderId);
        if (order == null)
        {
            return null;
        }
        var orderItems = new List<OrderItemDto>();
        foreach (var orderItem in order.OrderItems)
        {
            var name = "Unknown";
            var product = await productService.GetProductByArticleNumberAsync(orderItem.ArticleNumber);
            if (product != null)
            {
                name = product.Name;
            }
            orderItems.Add(new OrderItemDto(orderItem.ArticleNumber, name, orderItem.Quantity, orderItem.UnitPrice, orderItem.TotalAmount));
        }
        return new OrderDto(order.Id, order.CustomerId, order.TotalAmount, order.PaidAt, order.CreatedAt, orderItems);
    }
    
    public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
    {
        var orders = await orderRepository.GetAllAsync();
        return orders.Select(order => new OrderDto(order.Id, order.CustomerId, order.TotalAmount, order.PaidAt, order.CreatedAt, [])).ToList();
    }
    
    public async Task<IEnumerable<OrderDto>> GetOrdersAsync(Guid customerId)
    {
        var orders = await orderRepository.GetOrdersByCustomerIdAsync(customerId);
        return orders.Select(order => new OrderDto(order.Id, order.CustomerId, order.TotalAmount, order.PaidAt, order.CreatedAt, [])).ToList();
    }
    
    public async Task<bool> PayOrderAsync(Guid orderId)
    {
        var order = await orderRepository.GetAsync(x => x.Id == orderId);
        if (order == null)
        {
            return false;
        }
        order.PaidAt = DateTime.Now;
        await orderRepository.UpdateAsync(x => x.Id == orderId, order);
        return true;
    }
    
    public async Task<bool> DeleteOrderAsync(Guid orderId)
    {
        var order = await orderRepository.GetAsync(x => x.Id == orderId);
        if (order == null)
        {
            return false;
        }
        await orderRepository.DeleteAsync(x => x.Id == orderId);
        return true;
    }
}