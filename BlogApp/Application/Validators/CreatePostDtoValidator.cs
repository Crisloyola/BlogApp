using FluentValidation;
using BlogApp.Application.DTOs.Post;

namespace BlogApp.Application.Validators
{
    public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
    {

        public CreatePostDtoValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithName("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("Category is required")
                .MaximumLength(50).WithMessage("Category cannot exceed 50 characters");

            RuleFor(p => p.Content)
                .NotEmpty().WithMessage("Content is required");

            RuleFor(x => x.PublishDate)
                .NotEmpty().WithMessage("Publish date is required");

            RuleFor(p => p.AuthorId)
                .GreaterThan(0).WithMessage("Author ID must be greather than 0");

        }


    }
}