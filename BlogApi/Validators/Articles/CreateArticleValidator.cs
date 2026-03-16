using FluentValidation;
using BlogApi.DTOs.Articles;

namespace BlogApi.Validators.Articles;

public class CreateArticleValidator : AbstractValidator<CreateArticleRequest>
{
    public CreateArticleValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200);
        
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug is required")
            .MaximumLength(200);
        
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required");
        
        RuleFor(x => x.Excerpt)
            .MaximumLength(300)
            .When(x => x.Excerpt != null);
        
        RuleFor(x => x.AuthorId)
            .GreaterThan(0)
            .WithMessage("Author is required");
        
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category is required");
        
        RuleFor(x => x.Thumbnail)
            .MaximumLength(500)
            .When(x => x.Thumbnail != null);
        
        RuleFor(x => x.MetaTitle)
            .MaximumLength(70)
            .When(x => x.MetaTitle != null);
        
        RuleFor(x => x.MetaDescription)
            .MaximumLength(160)
            .When(x => x.MetaDescription != null);
        
        RuleFor(x => x.TagIds)
            .ForEach(tagId => tagId.GreaterThan(0))
            .When(x => x.TagIds.Count > 0);
    }
}
