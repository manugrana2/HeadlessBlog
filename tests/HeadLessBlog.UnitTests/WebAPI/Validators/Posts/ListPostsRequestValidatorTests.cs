using FluentValidation.TestHelper;
using HeadLessBlog.WebAPI.Models.Posts;
using HeadLessBlog.WebAPI.Validators.Posts;

namespace HeadLessBlog.UnitTests.WebAPI.Validators.Posts;

public class ListPostsRequestValidatorTests
{
    private readonly ListPostsRequestValidator _validator = new();

    [Fact]
    public void Should_Have_No_Errors_When_Valid_Request()
    {
        var request = new ListPostsRequest
        {
            Page = 1,
            PageSize = 20,
            SortBy = PostSortBy.CreatedAt,
            IsAscending = true
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_Page_Is_Zero()
    {
        var request = new ListPostsRequest
        {
            Page = 0,
            PageSize = 20,
            SortBy = PostSortBy.CreatedAt
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Out_Of_Range()
    {
        var request = new ListPostsRequest
        {
            Page = 1,
            PageSize = 150, 
            SortBy = PostSortBy.CreatedAt
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }
}
