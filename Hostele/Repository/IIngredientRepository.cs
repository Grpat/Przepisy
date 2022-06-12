using Hostele.Models;

namespace Hostele.Repository;

public interface IIngredientRepository:IRepository<Ingredient>
{
    void Update(Ingredient obj);
}