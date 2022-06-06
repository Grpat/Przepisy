using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Hostele.Repository;

public interface IRepository<T> where T : class
{
    Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> filter,string includeProperties = null);

  
    Task <IEnumerable<T>> GetAll2( Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null);
    
    Task <IEnumerable<T>> GetAll(string? includeProperties=null, Expression<Func<T, bool>> filter = null);


    Task<ICollection<TType>> GetSelected<TType>(Expression<Func<T, TType>> select) where TType : class;
    
    void Add(T entity);
    void Remove(T entity);
    bool CheckIfExists(Expression<Func<T, bool>> expr);

}