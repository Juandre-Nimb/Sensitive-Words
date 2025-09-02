using FluentValidation;
using SensitiveWordsClean.Application.DTOs;

namespace SensitiveWordsClean.Application.Validators;

public class CreateSensitiveWordDtoValidator : AbstractValidator<CreateSensitiveWordDto>
{
    public CreateSensitiveWordDtoValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty()
            .WithMessage("Word is required")
            .Length(1, 100)
            .WithMessage("Word must be between 1 and 100 characters")
            .Matches("^[a-zA-Z0-9\\s\\-_]+$")
            .WithMessage("Word can only contain letters, numbers, spaces, hyphens, and underscores");
    }
}

public class UpdateSensitiveWordDtoValidator : AbstractValidator<UpdateSensitiveWordDto>
{
    public UpdateSensitiveWordDtoValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty()
            .WithMessage("Word is required")
            .Length(1, 100)
            .WithMessage("Word must be between 1 and 100 characters")
            .Matches("^[a-zA-Z0-9\\s\\-_]+$")
            .WithMessage("Word can only contain letters, numbers, spaces, hyphens, and underscores");
    }
}

public class TextSanitizationRequestDtoValidator : AbstractValidator<TextSanitizationRequestDto>
{
    public TextSanitizationRequestDtoValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Text is required")
            .MaximumLength(10000)
            .WithMessage("Text cannot exceed 10,000 characters");

        RuleFor(x => x.ReplacementCharacter)
            .NotEmpty()
            .WithMessage("ReplacementCharacter is required")
            .Length(1, 5)
            .WithMessage("ReplacementCharacter must be between 1 and 5 characters");
    }
}
