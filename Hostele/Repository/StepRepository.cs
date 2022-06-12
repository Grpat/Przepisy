using Hostele.Data;
using Hostele.Models;
using JetBrains.Annotations;

namespace Hostele.Repository;

public class StepRepository:Repository<Step>,IStepRepository
{
    private readonly ApplicationDbContext _db;
    public StepRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    public void Update(Step obj)
    {
        _db.Steps.Update(obj);
    }
}