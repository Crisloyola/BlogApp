using AutoMapper;
using Microsoft.Extensions.Logging;
using BlogApp.Application.DTOs.Comment;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Exceptions;
using BlogApp.Domain.Ports.Out;

namespace BlogApp.Application.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentService> _logger;

    public CommentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CommentDto> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving comment with ID {CommentId}.", id);

        var comment = await _unitOfWork.Comments.GetWithPostAndAuthorAsync(id);
        if (comment == null)
        {
            _logger.LogWarning("Comment with ID {CommentId} not found.", id);
            throw new NotFoundException("Comment", id);
        }
        _logger.LogInformation("Comment on {CommentDate} found.", comment.CommentDate);
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<IEnumerable<CommentDto>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all comments.");
        var comments = await _unitOfWork.Comments.GetAllAsync();
        _logger.LogInformation("{CommentCount} comments retrieved.", comments.Count());
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }

    public async Task<CommentDto> CreateAsync(CreateCommentDto dto)
    {
        _logger.LogInformation("Creating a new comment for Post ID {PostID} on {CommentDate}", dto.PostId, dto.CommentDate);
        var post = await _unitOfWork.Posts.GetWithAuthorAsync(dto.PostId);
        if (post == null)
        {
            _logger.LogWarning("Post with ID {PostID} not found. Cannot create comment.", dto.PostId);
            throw new NotFoundException("Post", dto.PostId);
        }

        var comment = _mapper.Map<Comment>(dto);

        var createdComment = await _unitOfWork.Comments.CreateAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        var commentWithDetails = await _unitOfWork.Comments.GetWithPostAndAuthorAsync(createdComment.Id);
        _logger.LogInformation("Comment for Post ID {PostID} created successfully with ID {CommentID}.", dto.PostId, createdComment.Id);
        return _mapper.Map<CommentDto>(commentWithDetails!);
    }

    public async Task<CommentDto> UpdateAsync(int id, UpdateCommentDto dto)
    {
        var comment = await _unitOfWork.Comments.GetWithPostAndAuthorAsync(id);
        if (comment == null)
        {
            throw new NotFoundException("Comment", id);
        }

        var validStatuses = new[] { "Pending", "Approved", "Rejected" };
        if (!validStatuses.Contains(dto.Status))
        {
            throw new BusinessRuleException(
                "InvalidStatus",
                $"Status must be one of: {string.Join(", ", validStatuses)}");
        }

        _mapper.Map(dto, comment);

        var updatedComment = await _unitOfWork.Comments.UpdateAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CommentDto>(updatedComment);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Attempting to delete comment with ID {CommentID}.", id);
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);
        if (comment == null)
        {
            _logger.LogWarning("Comment with ID {CommentID} not found. Cannot delete.", id);
            throw new NotFoundException("Comment", id);
        }

        var result = await _unitOfWork.Comments.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Comment with ID {CommentID} deleted successfully.", id);
        return result;
    }

    public async Task<bool> ApproveAsync(int id)
    {
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);
        if (comment == null)
        {
            throw new NotFoundException("Comment", id);
        }

        if (!comment.CanBeDeleted())
        {
            throw new BusinessRuleException(
                "CannotApprove",
                "Only pending or approved comments can be modified.");
        }

        comment.Status = "Approved";

        await _unitOfWork.Comments.UpdateAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId)
    {
        var postExists = await _unitOfWork.Posts.ExistsAsync(postId);
        if (!postExists)
        {
            throw new NotFoundException("Post", postId);
        }

        var comments = await _unitOfWork.Comments.GetByPostIdAsync(postId);
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }

    public async Task<IEnumerable<CommentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new BusinessRuleException(
                "InvalidDateRange",
                "Start date must be before end date.");
        }

        var comments = await _unitOfWork.Comments.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }

    public async Task<IEnumerable<CommentDto>> GetByStatusAsync(string status)
    {
        var validStatuses = new[] { "Pending", "Approved", "Rejected" };
        if (!validStatuses.Contains(status))
        {
            throw new BusinessRuleException(
                "InvalidStatus",
                $"Status must be one of: {string.Join(", ", validStatuses)}");
        }

        var comments = await _unitOfWork.Comments.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }
}