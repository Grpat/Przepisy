using System.ComponentModel;
using Hostele.Data;
using Hostele.Models;

namespace Hostele.Repository;

public class UnitOfWork:IUnitOfWork

{
    private readonly ApplicationDbContext _db;
    public UnitOfWork(ApplicationDbContext db) 
    {
        _db = db;
        Recipe = new HostelRepository(_db);
        Category = new CategoryRepository(_db);
        Comment = new CommentRepository(_db);
        Step = new StepRepository(_db);
        Ingredient = new IngredientRepository(_db);

    }
    
    public IRecipeRepository Recipe { get; }
   public ICategoryRepository Category { get; }
   public ICommentRepository Comment { get; }
   public IStepRepository Step { get; }
   public IIngredientRepository Ingredient { get; }

   public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}