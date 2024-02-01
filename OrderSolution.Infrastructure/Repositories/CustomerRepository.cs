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
    
    public override async Task<bool> UpdateAsync(Expression<Func<CustomerEntity, bool>> predicate, CustomerEntity entity)
    {
        try
        {
            var customer = await context.Customers
                .Include(p => p.CustomerAddress)
                .Include(p => p.CustomerDetail)
                .FirstOrDefaultAsync(predicate);
            if (customer != null)
            {
                customer.CustomerAddress = entity.CustomerAddress;
                customer.CustomerDetail = entity.CustomerDetail;
                await context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }

        return false;
    }

    public override async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await context.Customers
            .Include(c => c.CustomerAddress)
            .Include(c => c.CustomerDetail)
            .ToListAsync();
    }
}