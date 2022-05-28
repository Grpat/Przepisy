using System.Linq.Expressions;
using Hostele.Data;
using Hostele.Models;
using Microsoft.EntityFrameworkCore;

namespace Hostele.Repository;

public class HostelRepository: Repository<Recipe>,IRecipeRepository
{
    private readonly ApplicationDbContext _db;
    public HostelRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    public async Task <IEnumerable<Recipe>> GetFiltered(string? includeProperties=null,string? searchString=null, string? category=null, string? sortOrder=null)
    {
        IQueryable<Recipe> query = _db.Recipes;
        if (includeProperties != null)
        {
            foreach (var includeProp in
                     includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        
        if (searchString != null)
        {
            var searchStringArr = searchString.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            query = query.Where(s => s.Ingrds != null && (s.RecipeName.ToUpper().Contains(searchString.ToUpper())
                                                          || s.Ingrds.Select(x => x.IngredientName.ToUpper())
                                                              .Contains(searchStringArr[0])));
            for (int i = 1; i < searchStringArr.Length - 1; i++)
            { 
                query=query.Where(s =>
                    s.Ingrds != null && s.Ingrds.Select(x => x.IngredientName).Contains(searchStringArr[i]));
            }
        }
        if (!String.IsNullOrEmpty(category))
        {
            query=query.Where(s => s.Category != null && s.Category.CategoryName==category);
        }
        switch (sortOrder)
        {
            case "name_desc":
                query = query.OrderByDescending(s => s.RecipeName);
                break;
            case "Date":
                query = query.OrderBy(s => s.CreatedDate);
                break;
            case "date_desc":
                query = query.OrderByDescending(s => s.CreatedDate);
                break;
            default:  // Name ascending 
                query = query.OrderBy(s => s.RecipeName);
                break;
        } 

        return await query.ToListAsync();
    }
    public void Update(Recipe obj)
    {
        _db.Recipes.Update(obj);
    }

    
}