using FluentValidation;
using HeadLessBlog.WebAPI.Models.Posts;
namespace HeadLessBlog.WebAPI.Validators.Posts;

public class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(10);
    }
}
