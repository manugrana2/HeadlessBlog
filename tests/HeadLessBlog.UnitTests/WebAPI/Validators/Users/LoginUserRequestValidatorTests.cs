using FluentValidation.TestHelper;
using HeadLessBlog.WebAPI.Models.Users;
using HeadLessBlog.WebAPI.Validators.Users;
using Xunit;

namespace HeadLessBlog.UnitTests.WebAPI.Validators.Users;

public class LoginUserRequestValidatorTests
{
    private readonly LoginUserRequestValidator _validator;

    public LoginUserRequestValidatorTests()
    {
        _validator = new LoginUserRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        // Arrange
        var model = new LoginUserRequest { Email = string.Empty, Password = "Password123" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var model = new LoginUserRequest { Email = "invalid-email", Password = "Password123" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        // Arrange
        var model = new LoginUserRequest { Email = "test@example.com", Password = string.Empty };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        // Arrange
        var model = new LoginUserRequest
        {
            Email = "test@example.com",
            Password = "ValidPassword123"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
