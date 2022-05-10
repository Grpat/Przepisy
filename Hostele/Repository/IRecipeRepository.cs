using Hostele.Models;

namespace Hostele.Repository;

public interface IRecipeRepository:IRepository<Recipe>
{
    void Update(Recipe obj);
    
}