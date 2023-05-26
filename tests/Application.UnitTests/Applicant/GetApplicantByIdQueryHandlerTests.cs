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
 
    public async Task Handle_GivenValidQuery_ShouldReturnApplicantDetailsDto2()
    {
        // Arrange
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
            History = new List<UserTimelineEvent>
        {
            new UserTimelineEvent
            {
                Note = "Event1",
                Time = DateTime.UtcNow,
                EventType = TimelineEventType.Created
            },
            new UserTimelineEvent
            {
                Note = "Add tags Tag1, Tag2",
                Time = DateTime.UtcNow,
                EventType = TimelineEventType.Note
            }
        },
            Tags = new List<UserTag>
        {
            new UserTag
            {
                Tag = new Tag { Name = "Tag1" }
            },
            new UserTag
            {
                Tag = new Tag { Name = "Tag2" }
            }
        }
        };

        var data = new List<Applicant> { applicant }.AsQueryable();

        var dbSetMock = new Mock<DbSet<Applicant>>();
        dbSetMock.As<IQueryable<Applicant>>().Setup(m => m.Provider).Returns(data.Provider);
        dbSetMock.As<IQueryable<Applicant>>().Setup(m => m.Expression).Returns(data.Expression);
        dbSetMock.As<IQueryable<Applicant>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbSetMock.As<IQueryable<Applicant>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbContextMock.Setup(x => x.Applicants).Returns(dbSetMock.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(applicant.Id);
        result.Avatar.Should().Be(applicant.Avatar);
        result.FirstName.Should().Be(applicant.FirstName);
        result.LastName.Should().Be(applicant.LastName);
        result.PhoneNumber.Should().Be(applicant.PhoneNumber);
        result.DateOfBirth.Should().Be(applicant.DateOfBirth);

        result.History.Should().HaveCount(2);
        result.History.ElementAt(0).Note.Should().Be("Event1");
        result.History.ElementAt(0).EventType.Should().Be(TimelineEventType.Created);
        result.History.ElementAt(1).Note.Should().Be("Event2");
        result.History.ElementAt(1).EventType.Should().Be(TimelineEventType.Note);

        result.Tags.Should().HaveCount(2);
        result.Tags.ElementAt(0).Name.Should().Be("Tag1");
        result.Tags.ElementAt(1).Name.Should().Be("Tag2");
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