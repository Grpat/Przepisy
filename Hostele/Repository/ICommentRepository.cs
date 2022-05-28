using Hostele.Models;

namespace Hostele.Repository;

public interface ICommentRepository:IRepository<Comment>
{
    void Update(Comment obj);
}
