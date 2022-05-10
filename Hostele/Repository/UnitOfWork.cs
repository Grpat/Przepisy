using System.ComponentModel;
using Hostele.Data;

namespace Hostele.Repository;

public class UnitOfWork:IUnitOfWork

{
    private readonly ApplicationDbContext _db;
    public UnitOfWork(ApplicationDbContext db) 
    {
        _db = db;
        Recipe = new HostelRepository(_db);
       

    }
   // public IHostelRepository Hostel{ get; private set; }


   public IRecipeRepository Recipe { get; }

   public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}