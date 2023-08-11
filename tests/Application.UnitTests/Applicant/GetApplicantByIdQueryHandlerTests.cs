using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Features.Applicants.Query.GetApplicantById;

namespace MRA.Jobs.Application.UnitTests.Applicant;

public class GetReviewerByIdCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetApplicantByIdQueryHandler(_dbContextMock.Object, Mapper);
    }

    private GetApplicantByIdQueryHandler _handler;


    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public async Task Handle_GivenValidQuery_ShouldReturnApplicantDetailsDto()
    {
        GetApplicantByIdQuery query = new GetApplicantByIdQuery { Id = Guid.NewGuid() };

        Domain.Entities.Applicant applicant = new Domain.Entities.Applicant
        {
            Id = query.Id,
            Avatar = "user_avatar",
            FirstName = "userFirstname",
            LastName = "userLastname",
            Email = "user@gmail.com",
            PhoneNumber = "123456789",
            DateOfBirth = DateTime.UtcNow
        };

        _dbContextMock.Setup(x => x.Applicants.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(applicant);

        // Act
        ApplicantDetailsDto result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(applicant.Id);
        result.Avatar.Should().Be(applicant.Avatar);
        result.FirstName.Should().Be(applicant.FirstName);
        result.LastName.Should().Be(applicant.LastName);
        result.PhoneNumber.Should().Be(applicant.PhoneNumber);
        result.DateOfBirth.Should().Be(applicant.DateOfBirth);
    }

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        GetApplicantByIdQuery query = new GetApplicantByIdQuery { Id = Guid.NewGuid() };
        _dbContextMock.Setup(a => a.Applicants.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Applicant)null);

        // Act + Assert 
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}