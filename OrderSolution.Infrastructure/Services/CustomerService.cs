using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Repositories;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<CustomerDto> CreateAsync(CustomerCreateDto dto)
    {
        var entity = dto.ToEntity();
        var result = await customerRepository.AddAsync(entity);
        return result == null! ? null! : result.ToDto();
    }
    
    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var result = await customerRepository.GetAsync(x => x.Id == id);
        return result == null! ? null! : result.ToDto();
    }
    
    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var result = await customerRepository.GetAllAsync();
        return result.Select(x => x.ToDto());
    }
    
    public async Task<bool> UpdateAsync(Guid id, CustomerCreateDto dto)
    {
        var entity = dto.ToEntity();
        var result = await customerRepository.UpdateAsync(x => x.Id == id, entity);
        return result;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await customerRepository.DeleteAsync(x => x.Id == id);
        return result;
    }
}