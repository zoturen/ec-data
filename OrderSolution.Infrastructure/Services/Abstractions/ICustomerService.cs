using OrderSolution.Infrastructure.Dtos;

namespace OrderSolution.Infrastructure.Services.Abstractions;

public interface ICustomerService
{
    Task<CustomerDto> CreateAsync(CustomerCreateDto dto);
    Task<CustomerDto> GetAsync(Guid id);
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<bool> UpdateAsync(Guid id, CustomerCreateDto dto);
    Task<bool> DeleteAsync(Guid id);
}