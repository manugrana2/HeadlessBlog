using FluentValidation.TestHelper;
using HeadLessBlog.WebAPI.Models.Comments;
using HeadLessBlog.WebAPI.Validators.Comments;

namespace HeadLessBlog.UnitTests.WebAPI.Validators.Comments;

public class UpdateCommentRequestValidatorTests
{
    private readonly UpdateCommentRequestValidator _validator = new();

    [Fact]
    public void Should_Have_No_Errors_When_Request_Is_Valid()
    {
        var request = new UpdateCommentRequest
        {
            Content = "Valid updated content."
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var request = new UpdateCommentRequest
        {
            Content = ""
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }
}
