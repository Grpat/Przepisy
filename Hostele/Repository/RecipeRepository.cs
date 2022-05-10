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

    public void Update(Recipe obj)
    {
        _db.Recipes.Update(obj);
    }

    
}