using Microsoft.EntityFrameworkCore;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Ports.Out;
using BlogApp.Infrastructure.Persistence.Context;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<Author?> GetByEmailAsync(string email)
        {
            return await _dbSet.
                FirstOrDefaultAsync(o => o.Email == email);
        }


        public async Task<Author?> GetWithPostsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Posts)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Author>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(o => EF.Functions.Like(o.FirstName, $"%{name}%") ||
                            EF.Functions.Like(o.LastName, $"%{name}%"))
                .ToListAsync();
        }

    }
}