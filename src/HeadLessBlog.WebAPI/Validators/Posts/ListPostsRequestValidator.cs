using FluentValidation;
using HeadLessBlog.WebAPI.Models.Posts;
namespace HeadLessBlog.WebAPI.Validators.Posts;

public class ListPostsRequestValidator : AbstractValidator<ListPostsRequest>
{
    public ListPostsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}
