using AutoMapper;
using BlogApp.Application.DTOs.Post;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Exceptions;
using BlogApp.Domain.Ports.Out;
using Microsoft.Extensions.Logging;

namespace BlogApp.Application.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PostService> _logger;

    public PostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PostDto> GetByIdAsync(int id)
    {
        var post = await _unitOfWork.Posts.GetWithAuthorAsync(id);
        if (post == null)
        {
            _logger.LogWarning("Post with ID {PostId} not found.", id);
            throw new NotFoundException("Post", id);
        }
        _logger.LogInformation("Post {PostTitle} found.", post.Title);
        return _mapper.Map<PostDto>(post);
    }

    public async Task<IEnumerable<PostDto>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all posts.");
        var posts = await _unitOfWork.Posts.GetAllAsync();
        _logger.LogInformation("{PostCount} posts retrieved.", posts.Count());
        return _mapper.Map<IEnumerable<PostDto>>(posts);
    }

    public async Task<PostDto> CreateAsync(CreatePostDto dto)
    {
        _logger.LogInformation("Creating a new post with title {PostTitle} for {AuthorID}", dto.Title, dto.AuthorId);
        var authorExists = await _unitOfWork.Authors.ExistsAsync(dto.AuthorId);
        if (!authorExists)
        {
            _logger.LogWarning("Author with ID {AuthorID} not found. Cannot create post.", dto.AuthorId);
            throw new NotFoundException("Author", dto.AuthorId);
        }

        if (dto.PublishDate < DateTime.Today)
        {
            throw new BusinessRuleException(
                "InvalidPublishDate",
                "Publish date cannot be in the past.");
        }

        var post = _mapper.Map<Post>(dto);

        var createdPost = await _unitOfWork.Posts.CreateAsync(post);
        await _unitOfWork.SaveChangesAsync();

        var postWithAuthor = await _unitOfWork.Posts.GetWithAuthorAsync(createdPost.Id);
        _logger.LogInformation("Post {PostTitle} created successfully with ID {PostID}.", createdPost.Title, createdPost.Id);
        return _mapper.Map<PostDto>(postWithAuthor!);
    }

    public async Task<PostDto> UpdateAsync(int id, UpdatePostDto dto)
    {
        var post = await _unitOfWork.Posts.GetWithAuthorAsync(id);
        if (post == null)
        {
            throw new NotFoundException("Post", id);
        }

        if (dto.PublishDate < DateTime.Today)
        {
            throw new BusinessRuleException(
                "InvalidPublishDate",
                "Publish date cannot be in the past.");
        }

        _mapper.Map(dto, post);

        var updatedPost = await _unitOfWork.Posts.UpdateAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PostDto>(updatedPost);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(id);
        if (post == null)
        {
            throw new NotFoundException("Post", id);
        }

        var comments = await _unitOfWork.Comments.GetByPostIdAsync(id);
        var pendingComments = comments.Where(c => c.Status == "Pending").ToList();

        if (pendingComments.Any())
        {
            throw new BusinessRuleException(
                "PostHasPendingComments",
                $"Cannot delete post with ID {id} because it has {pendingComments.Count} pending comments.");
        }

        var result = await _unitOfWork.Posts.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<PostDto>> GetByAuthorIdAsync(int authorId)
    {
        var authorExists = await _unitOfWork.Authors.ExistsAsync(authorId);
        if (!authorExists)
        {
            throw new NotFoundException("Author", authorId);
        }

        var posts = await _unitOfWork.Posts.GetByAuthorIdAsync(authorId);
        return _mapper.Map<IEnumerable<PostDto>>(posts);
    }

    public async Task<IEnumerable<PostDto>> GetByCategoryAsync(string category)
    {
        var posts = await _unitOfWork.Posts.GetByCategoryAsync(category);
        return _mapper.Map<IEnumerable<PostDto>>(posts);
    }
}