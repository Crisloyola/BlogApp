namespace BlogApp.Domain.Ports.Out
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository Authors { get; }
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }
        IUserRepository Users { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}