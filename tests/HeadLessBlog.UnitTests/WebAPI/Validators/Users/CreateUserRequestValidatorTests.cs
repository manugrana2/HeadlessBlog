using FluentValidation.TestHelper;
using HeadLessBlog.WebAPI.Models.Users;
using HeadLessBlog.WebAPI.Validators.Users;
using Xunit;

namespace HeadLessBlog.UnitTests.WebAPI.Validators.Users;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Username_Is_Empty()
    {
        var model = new CreateUserRequest { Username = "", Name = "Test", CountryCode = "US", Email = "test@test.com", Password = "password123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new CreateUserRequest { Username = "user", Name = "Test", CountryCode = "US", Email = "invalid-email", Password = "password123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var model = new CreateUserRequest
        {
            Username = "validuser",
            Name = "Valid Name",
            LastName = "Valid LastName",
            CountryCode = "US",
            Email = "validuser@example.com",
            Password = "strongpassword"
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
