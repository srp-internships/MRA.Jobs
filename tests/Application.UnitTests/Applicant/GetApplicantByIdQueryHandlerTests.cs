using MRA.Jobs.Application.Contracts.Applicant.Queries;

namespace MRA.Jobs.Application.UnitTests.Applicant;
using Domain.Entities;
using MRA.Jobs.Application.Features.Applicants.Query.GetApplicantById;

public class GetReviewerByIdCommandHandlerTests : BaseTestFixture
{
    private GetApplicantByIdQueryHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetApplicantByIdQueryHandler(_dbContextMock.Object, Mapper);
    }
    
    [Test]
    public async Task Handle_GivenValidQuery_ShouldReturnApplicantDetailsDto()
    {
        var query = new GetApplicantByIdQuery { Id = Guid.NewGuid() };

        var applicant = new Applicant
        {
            Id = query.Id,
            Avatar = "user_avatar",
            FirstName = "userFirstname",
            LastName = "userLastname",
            Email = "user@gmail.com",
            PhoneNumber = "123456789",
            DateOfBirth = DateTime.UtcNow,
        };

        _dbContextMock.Setup(x => x.Applicants.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(applicant);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(applicant.Id);
        result.Avatar.Should().Be(applicant.Avatar);
        result.FirstName.Should().Be(applicant.FirstName);
        result.LastName.Should().Be(applicant.LastName);
        result.PhoneNumber.Should().Be(applicant.PhoneNumber);
        result.DateOfBirth.Should().Be(applicant.DateOfBirth);
    }

    [Test]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetApplicantByIdQuery { Id = Guid.NewGuid() };
        _dbContextMock.Setup(a => a.Applicants.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Applicant)null);
        
        // Act + Assert 
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}