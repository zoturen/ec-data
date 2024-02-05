using OrderSolution.Infrastructure.Entities.Dbf;

namespace OrderSolution.Infrastructure.Repositories.Abstractions;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> AddImageAsync(string articleNumber, Image image);
    Task<bool> RemoveImageAsync(string articleNumber, Guid imageId);
}