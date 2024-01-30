using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class OrderRepository(CfContext context) : Repository<OrderEntity>(context), IOrderRepository
{
    public override async Task<OrderEntity?> GetAsync(Expression<Func<OrderEntity, bool>> predicate)
    {
        try
        {
            return await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(predicate);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }

        return null!;
    }
}