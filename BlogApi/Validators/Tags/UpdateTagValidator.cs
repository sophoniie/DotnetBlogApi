using FluentValidation;
using BlogApi.DTOs.Tags;

namespace BlogApi.Validators.Tags;

public class UpdateTagValidator : AbstractValidator<UpdateTagRequest>
{
    public UpdateTagValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(50);
    }
}
