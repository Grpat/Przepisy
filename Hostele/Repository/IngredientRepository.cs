using Hostele.Data;
using Hostele.Models;

namespace Hostele.Repository;

public class IngredientRepository:Repository<Ingredient>,IIngredientRepository
{
    private readonly ApplicationDbContext _db;
    public IngredientRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    public void Update(Ingredient obj)
    {
        _db.Ingrds.Update(obj);
    }
}