using BlogApp.Application.DTOs.Comment;

namespace BlogApp.Application.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> GetByIdAsync(int id);
        Task<IEnumerable<CommentDto>> GetAllAsync();
        Task<CommentDto> CreateAsync(CreateCommentDto commentDto);
        Task<CommentDto> UpdateAsync(int id, UpdateCommentDto commentDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ApproveAsync(int id);

        Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId);
        Task<IEnumerable<CommentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<CommentDto>> GetByStatusAsync(string status);
    }
}