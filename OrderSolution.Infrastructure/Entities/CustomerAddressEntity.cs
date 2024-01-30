using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSolution.Infrastructure.Entities;

public class CustomerAddressEntity
{
    [Key]
    [ForeignKey(nameof(CustomerEntity))]
    public Guid CustomerId { get; set; }
    [StringLength(50)]
    public string Street { get; set; } = null!;
    [StringLength(20)]
    public string City { get; set; } = null!;
    [StringLength(10)]
    public string ZipCode { get; set; } = null!;
    [StringLength(60)] // The United Kingdom of Great Britain and Northern Ireland = 56 characters
    public string Country { get; set; } = null!;

    public CustomerEntity Customer { get; set; } = null!;
}