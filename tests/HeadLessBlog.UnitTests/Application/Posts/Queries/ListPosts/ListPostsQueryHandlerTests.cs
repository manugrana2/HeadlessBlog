using FluentAssertions;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Application.Posts.Queries.ListPosts;
using Moq;

namespace HeadLessBlog.UnitTests.Application.Posts.Queries.ListPosts;

public class ListPostsQueryHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly ListPostsQueryHandler _handler;

    public ListPostsQueryHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _handler = new ListPostsQueryHandler(_postRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturn_Success_WhenRepositoryReturnsData()
    {
        // Arrange
        var expectedResult = new ListPostsResult
        {
            Posts = new List<PostSummaryDto>
            {
                new PostSummaryDto
                {
                    PostId = 1,
                    UserId = Guid.NewGuid(),
                    Title = "First Post",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                }
            },
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _postRepositoryMock
            .Setup(r => r.ListPostsAsync(
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var query = new ListPostsQuery
        {
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Posts.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ShouldReturn_Error_WhenExceptionOccurs()
    {
        // Arrange
        _postRepositoryMock
            .Setup(r => r.ListPostsAsync(
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database down"));

        var query = new ListPostsQuery
        {
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(ListPostsError.Unknown);
    }
}
