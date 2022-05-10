using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Hostele.Repository;

public interface IRepository<T> where T : class
{
    Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> filter,Func<IQueryable<T>,IIncludableQueryable<T, object>>? include = null);

    /*Task <IEnumerable<T>> GetAll(string? includeProperties=null);*/
    /*Task <IEnumerable<T>> GetAll( Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "");*/
    Task <IEnumerable<T>> GetAll(
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

    
    void Add(T entity);
    void Remove(T entity);
    bool CheckIfExists(Expression<Func<T, bool>> expr);

}