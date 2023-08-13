using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RankMyBeerDomain.Models;
using RankMyBeerInfrastructure.Context;
using RankMyBeerInfrastructure.Extensions;

namespace RankMyBeerInfrastructure.Repositories;
public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly RankMyBeerContext rankMyBeerContext;
    private readonly DbSet<T> dbSet;

    public BaseRepository(RankMyBeerContext rankMyBeerContext)
    {
        this.rankMyBeerContext = rankMyBeerContext;
        this.dbSet = rankMyBeerContext.Set<T>();
    }

    public async Task Delete(T entityToDelete)
    {
        if (rankMyBeerContext.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
        await rankMyBeerContext.SaveChangesAsync();
    }

    public async Task SaveAsync()
    {
        await rankMyBeerContext.SaveChangesAsync();
    }

    public virtual async Task<PagedResult<T>> Get(
        int? page,
        int? pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            var orderedQuery = orderBy(query);
            return await orderedQuery.GetPaged(page, pageSize);
        }
        else
        {
            return await query.GetPaged(page, pageSize);
        }
    }

    public async Task<T?> GetByID(Guid id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task Insert(T entity)
    {
        await dbSet.AddAsync(entity);
        await rankMyBeerContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        dbSet.Attach(entity);
        dbSet.Entry(entity).State = EntityState.Modified;
        await rankMyBeerContext.SaveChangesAsync();
    }
}