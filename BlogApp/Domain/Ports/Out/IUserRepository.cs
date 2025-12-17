using BlogApp.Domain.Entities;
using System.Threading.Tasks;
namespace BlogApp.Domain.Ports.Out
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
    }
}