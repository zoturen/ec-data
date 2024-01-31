using System.ComponentModel.DataAnnotations;
using OrderSolution.Infrastructure.Entities;

namespace OrderSolution.Infrastructure.Dtos;

public class CustomerCreateDto
{
    [MaxLength(50)]
    [Required]
    public string FirstName { get; set; } = null!;
    [MaxLength(50)]
    [Required]
    public string LastName { get; set; } = null!;
    [MaxLength(100)]
    [Required]
    public string Email { get; set; } = null!;
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    [StringLength(50)]
    [Required]
    public string Street { get; set; } = null!;
    [StringLength(20)]
    [Required]
    public string City { get; set; } = null!;
    [StringLength(10)]
    [Required]
    public string ZipCode { get; set; } = null!;
    [StringLength(60)]
    [Required]
    public string Country { get; set; } = null!;
}

public static partial class DtoExtensions
{
    public static CustomerEntity ToEntity(this CustomerCreateDto dto)
    {
        var entity = new CustomerEntity
        {
            Id = Guid.NewGuid(),
        };

        var customerAddress = new CustomerAddressEntity
        {
            CustomerId = entity.Id,
            Street = dto.Street,
            City = dto.City,
            ZipCode = dto.ZipCode,
            Country = dto.Country,
        };
        
        var customerDetail = new CustomerDetailEntity
        {
            CustomerId = entity.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
        };
        
        entity.CustomerAddress = customerAddress;
        entity.CustomerDetail = customerDetail;
        return entity;
    }
}