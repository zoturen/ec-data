using OrderSolution.Infrastructure.Entities;

namespace OrderSolution.Infrastructure.Dtos;

public record CustomerDto(Guid Id, string FirstName, string LastName, string Email, string? PhoneNumber, string Street, string City, string ZipCode, string Country);

public static partial class DtoExtensions
{
    public static CustomerDto ToDto(this CustomerEntity customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.CustomerDetail.FirstName, 
            customer.CustomerDetail.LastName, 
            customer.CustomerDetail.Email, 
            customer.CustomerDetail.PhoneNumber,
            customer.CustomerAddress.Street, 
            customer.CustomerAddress.City,
            customer.CustomerAddress.ZipCode,
            customer.CustomerAddress.Country);
    }
}