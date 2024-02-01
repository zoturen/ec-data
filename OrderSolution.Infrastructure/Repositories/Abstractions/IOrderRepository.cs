using OrderSolution.Infrastructure.Entities;

namespace OrderSolution.Infrastructure.Repositories.Abstractions;

public interface IOrderRepository : IRepository<OrderEntity>
{
    Task<IEnumerable<OrderEntity>> GetOrdersByCustomerIdAsync(Guid customerId);
}