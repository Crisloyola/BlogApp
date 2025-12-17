using Microsoft.EntityFrameworkCore;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Ports.Out;
using BlogApp.Infrastructure.Persistence.Context;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
        {
            return await _dbSet
                .Where(a => a.PostId == postId)
                .Include(a => a.Post)
                    .ThenInclude(p => p!.Author)
                .OrderByDescending(a => a.CommentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(a => a.CommentDate >= startDate && a.CommentDate <= endDate)
                .Include(a => a.Post)
                    .ThenInclude(p => p!.Author)
                .OrderBy(a => a.CommentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(a => a.Status == status)
                .Include(a => a.Post)
                    .ThenInclude(p => p!.Author)
                .OrderByDescending(a => a.CommentDate)
                .ToListAsync();
        }

        public async Task<Comment?> GetWithPostAndAuthorAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Post)
                    .ThenInclude(p => p!.Author)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}