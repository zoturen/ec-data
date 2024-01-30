using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class CustomerDetailRepository(CfContext context) : Repository<CustomerDetailEntity>(context), ICustomerDetailRepository
{
    
}