using Microsoft.EntityFrameworkCore;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Ports.Out;
using BlogApp.Infrastructure.Persistence.Context;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetByAuthorIdAsync(int authorId)
        {
            return await _dbSet
                .Where(p => p.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<Post?> GetWithAuthorAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(p => p.Category == category)
                .ToListAsync();
        }

    }
}