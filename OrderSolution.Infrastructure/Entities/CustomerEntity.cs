using System.ComponentModel.DataAnnotations;

namespace OrderSolution.Infrastructure.Entities;

public class CustomerEntity
{
    [Key]
    public Guid Id { get; set; }

    public CustomerAddressEntity CustomerAddress { get; set; } = null!;
    public CustomerDetailEntity CustomerDetail { get; set; } = null!;
}