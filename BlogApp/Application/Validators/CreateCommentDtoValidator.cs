using System.Data;
using FluentValidation;
using BlogApp.Application.DTOs.Comment;

namespace BlogApp.Application.Validators
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(a => a.PostId)
                .GreaterThan(0).WithMessage("Post ID must be greater than 0");

            RuleFor(a => a.CommentDate)
                .NotEmpty().WithMessage("Comment date is required");

            RuleFor(a => a.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(1000).WithMessage("Content cannot exceed 1000 characters");

            RuleFor(a => a.UserName)
                .MaximumLength(100).WithMessage("UserName cannot exceed 100 characters");
        }
    }
}