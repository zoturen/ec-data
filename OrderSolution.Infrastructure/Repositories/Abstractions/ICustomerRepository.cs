using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public interface ICustomerRepository : IRepository<CustomerEntity>
{
    
}