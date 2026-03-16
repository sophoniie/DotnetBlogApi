using FluentValidation;
using BlogApi.DTOs.Tags;
using BlogApi.Repositories.Tags;

namespace BlogApi.Validators.Tags;

public class CreateTagValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagValidator(ITagRepository tagRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(50)
            .MustAsync(async (name, cancellation) => 
                !await tagRepository.NameExistsAsync(name))
            .WithMessage("Tag name already exists");
    }
}
