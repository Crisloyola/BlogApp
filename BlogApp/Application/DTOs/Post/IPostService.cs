using BlogApp.Application.DTOs.Post;

namespace BlogApp.Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDto> GetByIdAsync(int id);
        Task<IEnumerable<PostDto>> GetAllAsync();
        Task<PostDto> CreateAsync(CreatePostDto postDto);
        Task<PostDto> UpdateAsync(int id, UpdatePostDto postDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<PostDto>> GetByAuthorIdAsync(int authorId);
        Task<IEnumerable<PostDto>> GetByCategoryAsync(string category);
    }
}