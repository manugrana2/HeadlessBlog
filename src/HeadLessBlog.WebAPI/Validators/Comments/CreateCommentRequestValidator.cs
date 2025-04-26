using FluentValidation;
using HeadLessBlog.WebAPI.Requests.Comments;

namespace HeadLessBlog.WebAPI.Validators.Comments;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.PostId)
            .GreaterThan(0)
            .WithMessage("PostId must be greater than 0.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(5000)
            .WithMessage("Content must not exceed 5000 characters.");
    }
}
