using Hostele.Models;

namespace Hostele.Repository;

public interface ICategoryRepository:IRepository<Category>
{
    void Update(Category obj);
}