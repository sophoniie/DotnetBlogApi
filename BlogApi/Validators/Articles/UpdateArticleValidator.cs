using FluentValidation;
using BlogApi.DTOs.Articles;

namespace BlogApi.Validators.Articles;

public class UpdateArticleValidator : AbstractValidator<UpdateArticleRequest>
{
    public UpdateArticleValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200);
        
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required");
        
        RuleFor(x => x.Thumbnail)
            .MaximumLength(500)
            .When(x => x.Thumbnail != null);
    }
}
