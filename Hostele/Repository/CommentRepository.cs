using Hostele.Data;
using Hostele.Models;

namespace Hostele.Repository;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    private readonly ApplicationDbContext _db;

    public CommentRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Comment obj)
    {
        _db.Comments.Update(obj);
    }
}