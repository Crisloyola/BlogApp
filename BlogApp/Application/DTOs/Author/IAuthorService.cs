using BlogApp.Application.DTOs.Author;

namespace BlogApp.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetByIdAsync(int id);
        Task<IEnumerable<AuthorDto?>> GetAllAsync();
        Task<AuthorDto> CreateAsync(CreateAuthorDto authorDto);
        Task<AuthorDto> UpdateAsync(int id, UpdateAuthorDto authorDto);

        Task<bool> DeleteAsync(int id);

        Task<AuthorDto?> GetByEmailAsync(string email);
        Task<IEnumerable<AuthorDto>> SearchByNameAsync(string name);
    }
}