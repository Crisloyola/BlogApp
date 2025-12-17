using BlogApp.Domain.Entities;

namespace BlogApp.Domain.Ports.Out
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<Author?> GetByEmailAsync(string email);
        Task<Author?> GetWithPostsAsync(int id);
        Task<IEnumerable<Author>> SearchByNameAsync(string name);
    }


}