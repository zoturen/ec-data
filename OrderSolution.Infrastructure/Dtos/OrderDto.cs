namespace OrderSolution.Infrastructure.Dtos;

public record OrderDto(Guid Id, Guid CustomerId, decimal TotalAmount, DateTime PaidAt, DateTime CreatedAt, IEnumerable<OrderItemDto> OrderItems);

public record OrderItemDto(string ArticleNumber, string Name, int Quantity, decimal UnitPrice, decimal TotalAmount);