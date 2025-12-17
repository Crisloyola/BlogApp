using BlogApp.Domain.Entities;

namespace BlogApp.Domain.Ports.Out
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetByAuthorIdAsync(int authorId);
        Task<Post?> GetWithAuthorAsync(int id);
        Task<IEnumerable<Post>> GetByCategoryAsync(string category);

    }
}