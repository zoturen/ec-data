using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class CustomerRepository(CfContext context) : Repository<CustomerEntity>(context), ICustomerRepository
{
    public override async Task<CustomerEntity?> GetAsync(Expression<Func<CustomerEntity, bool>> predicate)
    {
        try
        {
            return await context.Customers
                .Include(c => c.CustomerAddress)
                .Include(c => c.CustomerDetail)
                .FirstOrDefaultAsync(predicate);
            
        }
        catch (Exception e)
        {
           Debug.WriteLine($"ERROR: {e.Message}");
        }

        return null!;
    }
}