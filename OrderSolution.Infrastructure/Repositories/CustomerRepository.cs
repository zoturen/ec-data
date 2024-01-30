using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities;

namespace OrderSolution.Infrastructure.Repositories;

public class CustomerRepository(CfContext context) : Repository<CustomerEntity>(context), ICustomerRepository
{
    
}