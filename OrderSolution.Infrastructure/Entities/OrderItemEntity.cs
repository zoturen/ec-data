using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSolution.Infrastructure.Entities;

public class OrderItemEntity
{
    public Guid OrderId { get; set; } // PK
    public string ArticleNumber { get; set; } = null!; // PK
    public int Quantity { get; set; }
    [Column(TypeName = "Numeric(10,2)")]
    public decimal UnitPrice { get; set; }
    [Column(TypeName = "Numeric(10,2)")]
    public decimal TotalAmount { get; set; }
    
    public OrderEntity Order { get; set; } = null!;
}