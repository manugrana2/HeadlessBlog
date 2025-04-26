using FluentValidation;
using HeadLessBlog.WebAPI.Models.Comments;

namespace HeadLessBlog.WebAPI.Validators.Comments;

public class ListCommentsRequestValidator : AbstractValidator<ListCommentsRequest>
{
    public ListCommentsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.CreatedFrom)
            .LessThanOrEqualTo(x => x.CreatedTo)
            .When(x => x.CreatedFrom.HasValue && x.CreatedTo.HasValue)
            .WithMessage("'CreatedFrom' must be earlier than or equal to 'CreatedTo'.");
    }
}
