using System.Linq.Expressions;

namespace OrderSolution.Infrastructure.Repositories.Abstractions;

public interface IRepository <TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity);
}