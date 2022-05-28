using Hostele.Models;

namespace Hostele.Repository;

public interface IRecipeRepository:IRepository<Recipe>
{
    Task<IEnumerable<Recipe>> GetFiltered(string? includeProperties = null, string? searchString = null ,string? category=null, string? sortOrder=null);
    void Update(Recipe obj);
    
}