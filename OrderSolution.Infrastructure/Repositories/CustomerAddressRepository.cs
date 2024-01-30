using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class CustomerAddressRepository(CfContext context) : Repository<CustomerAddressEntity>(context), ICustomerAddressRepository
{
    
}