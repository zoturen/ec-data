using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSolution.Infrastructure.Entities;

public class OrderEntity
{
    [Key]
    public Guid Id { get; set; }
    [Column(TypeName = "Numeric(10,2)")]
    public decimal TotalAmount { get; set; }
    public DateTime PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    [ForeignKey(nameof(CustomerEntity))]
    public Guid CustomerId { get; set; }
    
    public CustomerEntity Customer { get; set; } = null!;
    public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
}