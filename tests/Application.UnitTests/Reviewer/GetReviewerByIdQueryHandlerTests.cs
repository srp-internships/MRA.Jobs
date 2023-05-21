using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerById;

namespace MRA.Jobs.Application.UnitTests.Reviewer;
using Domain.Entities;

public class GetReviewerByIdQueryHandlerTests  : BaseTestFixture
{
    private GetReviewerByIdQueryHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetReviewerByIdQueryHandler(_dbContextMock.Object, Mapper);
    }
    
    [Test]
    public async Task Handle_GivenValidQuery_ShouldReturnApplicantDetailsDto()
    {
        var query = new GetReviewerByIdQuery { Id = Guid.NewGuid() };

        var reviewer = new Reviewer
        {
            Id = query.Id,
            Avatar = "user_avatar",
            FirstName = "userFirstname",
            LastName = "userLastname",
            Email = "user@gmail.com",
            PhoneNumber = "123456789",
            DateOfBrith = DateTime.UtcNow,
        };

        _dbContextMock.Setup(x => x.Reviewers.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reviewer);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(reviewer.Id);
        result.Avatar.Should().Be(reviewer.Avatar);
        result.FirstName.Should().Be(reviewer.FirstName);
        result.LastName.Should().Be(reviewer.LastName);
        result.PhoneNumber.Should().Be(reviewer.PhoneNumber);
        result.DateOfBrith.Should().Be(reviewer.DateOfBrith);
    }

    [Test]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetReviewerByIdQuery { Id = Guid.NewGuid() };
        _dbContextMock.Setup(a => a.Reviewers.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reviewer)null);
        
        // Act + Assert 
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

}