using System.Linq.Expressions;
using RankMyBeerDomain.Models;

namespace RankMyBeerInfrastructure.Repositories;
public interface IRepository<T> where T : class
{
    Task Delete(T entityToDelete);
    Task<PagedResult<T>> Get(
        int? page,
        int? pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "");
    Task<T?> GetByID(Guid id);
    Task Insert(T entity);
    Task Update(T entity);
    Task SaveAsync();
}
