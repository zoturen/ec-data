using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSolution.Infrastructure.Entities;

public class CustomerDetailEntity
{
    [Key]
    [ForeignKey(nameof(CustomerEntity))]
    public Guid CustomerId { get; set; }
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    [StringLength(100)]
    public string Email { get; set; } = null!;
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
}