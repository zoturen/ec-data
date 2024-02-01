namespace OrderSolution.Infrastructure.Dtos;

public class OrderCreateDto
{
    public Guid CustomerId { get; set; }
    public List<OrderItemCreateDto> OrderItems { get; set; } = new();
}

public record OrderItemCreateDto(string ArticleNumber, int Quantity);