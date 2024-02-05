using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public abstract class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
{
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR: " + e.Message);
        }

        return null!;
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = context.Set<TEntity>().FirstOrDefault(predicate);
            if (entity != null)
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR: " + e.Message);
        }

        return false;
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return await context.Set<TEntity>().AnyAsync(predicate);
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR: " + e.Message);
        }

        return false;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            return await context.Set<TEntity>().ToListAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR: " + e.Message);
        }

        return [];
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR: " + e.Message);
        }

        return null!;
    }

    public virtual async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity)
    {
        try
        {
            var existingEntity = await context.Set<TEntity>().SingleOrDefaultAsync(predicate);
            if (existingEntity != null)
            {
                context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR: " + e.Message);
        }

        return false;
    }
}