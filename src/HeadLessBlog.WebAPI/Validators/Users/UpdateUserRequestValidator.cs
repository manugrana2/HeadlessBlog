using FluentValidation;
using HeadLessBlog.WebAPI.Models.Users;

namespace HeadLessBlog.WebAPI.Validators.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("LastName must not exceed 100 characters.")
            .When(x => x.LastName != null);

        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("Country code is required.")
            .Length(2).WithMessage("Country code must be exactly 2 characters.");
    }
}
