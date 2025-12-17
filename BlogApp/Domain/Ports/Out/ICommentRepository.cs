using BlogApp.Domain.Entities;

namespace BlogApp.Domain.Ports.Out
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdAsync(int postId);
        Task<IEnumerable<Comment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Comment>> GetByStatusAsync(string status);
        Task<Comment?> GetWithPostAndAuthorAsync(int id);
    }
}