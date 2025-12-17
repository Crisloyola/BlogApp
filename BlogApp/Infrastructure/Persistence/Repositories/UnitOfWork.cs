using Microsoft.EntityFrameworkCore.Storage;
using BlogApp.Domain.Ports.Out;
using BlogApp.Infrastructure.Persistence.Context;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public IAuthorRepository Authors { get; }
        public IPostRepository Posts { get; }
        public ICommentRepository Comments { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(ApplicationDbContext context,
                          IAuthorRepository authorRepository,
                          IPostRepository postRepository,
                          ICommentRepository commentRepository,
                          IUserRepository userRepository
                          )
        {
            _context = context;
            Authors = authorRepository;
            Posts = postRepository;
            Comments = commentRepository;
            Users = userRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}