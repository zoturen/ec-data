using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSolution.Infrastructure.Entities;

public class OrderItemEntity
{
    public Guid OrderId { get; set; } // PK
    public Guid ArticleNumber { get; set; } // PK
    public int Quantity { get; set; }
    [Column(TypeName = "Numeric(7,2)")]
    public decimal UnitPrice { get; set; }
    [Column(TypeName = "Numeric(7,2)")]
    public decimal TotalAmount { get; set; }
}