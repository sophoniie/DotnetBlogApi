using FluentValidation;
using BlogApi.DTOs.Categories;
using BlogApi.Repositories.Categories;

namespace BlogApi.Validators.Categories;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100);
        
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug is required")
            .MaximumLength(150)
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must contain only lowercase letters, numbers, and hyphens")
            .MustAsync(async (slug, cancellation) => 
                !await categoryRepository.SlugExistsAsync(slug))
            .WithMessage("Slug already exists");
        
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description != null);
    }
}
