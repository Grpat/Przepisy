using Hostele.Models;

namespace Hostele.Repository;

public interface IStepRepository:IRepository<Step>
{
    void Update(Step obj);
}