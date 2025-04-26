using FluentAssertions;
using HeadLessBlog.Application.Comments.Queries.ListComments;
using HeadLessBlog.Application.Common.Interfaces;
using Moq;

namespace HeadLessBlog.UnitTests.Application.Comments.Queries.ListComments;

public class ListCommentsQueryHandlerTests
{
    private readonly Mock<ICommentRepository> _mockCommentRepository;
    private readonly ListCommentsQueryHandler _handler;

    public ListCommentsQueryHandlerTests()
    {
        _mockCommentRepository = new Mock<ICommentRepository>();
        _handler = new ListCommentsQueryHandler(_mockCommentRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_ListCommentsResult_When_Successful()
    {
        // Arrange
        var query = new ListCommentsQuery
        {
            Page = 1,
            PageSize = 10
        };

        var expectedResult = new ListCommentsResult
        {
            Comments = new List<CommentDto>
            {
                new CommentDto
                {
                    CommentId = 1,
                    PostId = 1,
                    UserId = Guid.NewGuid(),
                    Content = "Test comment",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Now
                }
            },
            TotalCount = 1
        };

        _mockCommentRepository
            .Setup(x => x.ListCommentsAsync(
                It.IsAny<Guid?>(),
                It.IsAny<int?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsT0.Should().BeTrue(); // Is Success
        result.AsT0.Comments.Should().HaveCount(1);
        result.AsT0.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_Exception_Occurs()
    {
        // Arrange
        var query = new ListCommentsQuery
        {
            Page = 1,
            PageSize = 10
        };

        _mockCommentRepository
            .Setup(x => x.ListCommentsAsync(
                It.IsAny<Guid?>(),
                It.IsAny<int?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database failure"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsT1.Should().BeTrue(); // Is Error
        result.AsT1.Error.Should().Be(ListCommentsError.Unknown);
    }
}
