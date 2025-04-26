using FluentAssertions;
using FluentValidation.TestHelper;
using HeadLessBlog.WebAPI.Models.Posts;
using HeadLessBlog.WebAPI.Validators.Posts;

namespace HeadLessBlog.UnitTests.WebAPI.Validators.Posts;

public class CreatePostRequestValidatorTests
{
    private readonly CreatePostRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var request = new CreatePostRequest
        {
            Title = "",
            Content = "This is valid content."
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var request = new CreatePostRequest
        {
            Title = "Valid Title",
            Content = ""
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Short()
    {
        var request = new CreatePostRequest
        {
            Title = "Hey",
            Content = "This is valid content."
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Long()
    {
        var longTitle = new string('a', 201);
        var request = new CreatePostRequest
        {
            Title = longTitle,
            Content = "This is valid content."
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Too_Short()
    {
        var request = new CreatePostRequest
        {
            Title = "Valid Title",
            Content = "short"
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        var request = new CreatePostRequest
        {
            Title = "Valid Title",
            Content = "This is a valid content that has enough length."
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
