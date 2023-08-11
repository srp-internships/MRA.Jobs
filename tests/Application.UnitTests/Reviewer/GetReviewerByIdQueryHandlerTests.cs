using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;
using MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerById;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class GetReviewerByIdQueryHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetReviewerByIdQueryHandler(_dbContextMock.Object, Mapper);
    }

    private GetReviewerByIdQueryHandler _handler;

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public async Task Handle_GivenValidQuery_ShouldReturnApplicantDetailsDto()
    {
        GetReviewerByIdQuery query = new GetReviewerByIdQuery { Id = Guid.NewGuid() };

        Domain.Entities.Reviewer reviewer = new Domain.Entities.Reviewer
        {
            Id = query.Id,
            Avatar = "user_avatar",
            FirstName = "userFirstname",
            LastName = "userLastname",
            Email = "user@gmail.com",
            PhoneNumber = "123456789",
            DateOfBirth = DateTime.UtcNow
        };

        _dbContextMock.Setup(x => x.Reviewers.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reviewer);

        // Act
        ReviewerDetailsDto result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(reviewer.Id);
        result.Avatar.Should().Be(reviewer.Avatar);
        result.FirstName.Should().Be(reviewer.FirstName);
        result.LastName.Should().Be(reviewer.LastName);
        result.PhoneNumber.Should().Be(reviewer.PhoneNumber);
        result.DateOfBirth.Should().Be(reviewer.DateOfBirth);
    }

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        GetReviewerByIdQuery query = new GetReviewerByIdQuery { Id = Guid.NewGuid() };
        _dbContextMock.Setup(a => a.Reviewers.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Reviewer)null);

        // Act + Assert 
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}