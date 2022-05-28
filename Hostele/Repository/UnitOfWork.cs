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

    }
    
    public IRecipeRepository Recipe { get; }
   public ICategoryRepository Category { get; }
   public ICommentRepository Comment { get; }

   public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}