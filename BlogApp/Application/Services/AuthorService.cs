using AutoMapper;
using Microsoft.Extensions.Logging;
using BlogApp.Application.DTOs.Author;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Exceptions;
using BlogApp.Domain.Ports.Out;

namespace BlogApp.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthorService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto authorDto)
        {
            _logger.LogInformation("Creating a new author with email {AuthorEmail}", authorDto.Email);
            var existingAuthor = await _unitOfWork.Authors.GetByEmailAsync(authorDto.Email);
            if (existingAuthor != null)
            {
                _logger.LogWarning("Author with email {AuthorEmail} already exists. Cannot create duplicate.", authorDto.Email);
                throw new DuplicateEntityException("Author", "Email", authorDto.Email);
            }
            var author = _mapper.Map<Author>(authorDto);
            var createdAuthor = await _unitOfWork.Authors.CreateAsync(author);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Author with email {AuthorEmail} created successfully with ID {AuthorID}.", authorDto.Email, createdAuthor.Id);
            return _mapper.Map<AuthorDto>(createdAuthor);
        }

        public async Task<bool> DeleteAsync(int id)
        {

            var author = await _unitOfWork.Authors.GetWithPostsAsync(id);
            if (author == null)
            {
                throw new NotFoundException("Author", id);
            }

            if (author.Posts.Any())
            {
                throw new BusinessRuleException(
                    "Author has posts",
                    $"Cannot delete author with ID {id} because they have posts {author.Posts.Count} registered. "
                );
            }

            var result = await _unitOfWork.Authors.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return result;

        }

        public async Task<IEnumerable<AuthorDto?>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all authors.");
            var authors = await _unitOfWork.Authors.GetAllAsync();

            _logger.LogInformation("{AuthorCount} authors retrieved.", authors.Count());
            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task<AuthorDto?> GetByEmailAsync(string email)
        {
            var author = await _unitOfWork.Authors.GetByEmailAsync(email);
            return author != null ? null : _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving author with ID {AuthorId}.", id);
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found.", id);
                throw new NotFoundException("Author", id);
            }
            _logger.LogInformation("Author {AuthorName} found.", author.FirstName);
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<IEnumerable<AuthorDto>> SearchByNameAsync(string name)
        {
            var author = await _unitOfWork.Authors.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<AuthorDto>>(author);
        }

        public async Task<AuthorDto> UpdateAsync(int id, UpdateAuthorDto authorDto)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
            {
                throw new NotFoundException("Author", id);
            }

            var existingAuthor = await _unitOfWork.Authors.GetByEmailAsync(authorDto.Email);
            if (existingAuthor != null && existingAuthor.Id != id)
            {
                throw new DuplicateEntityException("Author", "Email", authorDto.Email);
            }

            _mapper.Map(authorDto, author);
            var updatedAuthor = await _unitOfWork.Authors.UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AuthorDto>(updatedAuthor);
        }
    }
}