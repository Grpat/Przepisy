﻿using System.Linq.Expressions;
using Hostele.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Hostele.Repository;

public class Repository<T>: IRepository<T> where T:class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
    }
    public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> filter,Func<IQueryable<T>,IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = dbSet;
        if (include != null)
        {
            query = include(query);
        }

        query=query.Where(filter);
        return await query.FirstOrDefaultAsync();

    }

    public async Task<IEnumerable<T>> GetAll(
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
    )
    {
        IQueryable<T> query = dbSet;
        
        if (include != null)
        {
            query = include(query);
            return await query.ToListAsync();
        }

        return await query.ToListAsync();
    }

    /*public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
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
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }*/

    /*public async Task <IEnumerable<T>> GetAll(string? includeProperties=null)
    {
        IQueryable<T> query = dbSet;
        if (includeProperties != null)
        {
            foreach (var includeProp in
                     includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return await query.ToListAsync();
    }
    */

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }
    public bool CheckIfExists(Expression<Func<T, bool>> expr)
    {
        IQueryable<T> query = dbSet;
        query=query.Where(expr);
        return query.Any();
        
    }
}