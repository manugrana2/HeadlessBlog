using FluentValidation.TestHelper;
using HeadLessBlog.WebAPI.Requests.Comments;
using HeadLessBlog.WebAPI.Validators.Comments;

namespace HeadLessBlog.UnitTests.WebAPI.Validators.Comments;

public class CreateCommentRequestValidatorTests
{
    private readonly CreateCommentRequestValidator _validator = new();

    [Fact]
    public void Should_Have_No_Errors_When_Request_Is_Valid()
    {
        var request = new CreateCommentRequest
        {
            PostId = 1,
            Content = "A valid comment."
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_PostId_Is_Zero()
    {
        var request = new CreateCommentRequest
        {
            PostId = 0,
            Content = "Valid content."
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.PostId);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var request = new CreateCommentRequest
        {
            PostId = 1,
            Content = ""
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }
}
