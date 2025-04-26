using FluentValidation;
using HeadLessBlog.WebAPI.Models.Comments;

namespace HeadLessBlog.WebAPI.Validators.Comments;

public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
{
    public UpdateCommentRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(5000)
            .WithMessage("Content must not exceed 5000 characters.");
    }
}
