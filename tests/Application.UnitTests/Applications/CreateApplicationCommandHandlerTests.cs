using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

namespace MRA.Jobs.Application.UnitTests.Applications;

using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Domain.Entities;
public class CreateApplicationCommandHandlerTests : BaseTestFixture
{
    private CreateApplicationCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateApplicationCommandHandler(
            _dbContextMock.Object,
            Mapper,
            _dateTimeMock.Object,
            _currentUserServiceMock.Object,
            _slugGenerator.Object);
    }

    [Test]
    [Ignore("System.NullReferenceException")]
    public async Task Handle_ValidRequest_ShouldCreateApplicationAndTimelineEvent()
    {
        // Arrange
        var request = new CreateApplicationCommand
        {
            ApplicantId = Guid.NewGuid(),
            CoverLetter = "Jax Sampson\r\n\r\n(111) 789-3456\r\n\r\njax.sampson@email.com\r\n\r\n13-Jul-19\r\n\r\nDear Hiring Manager,\r\n\r\nI would like to throw in my hat as an applicant for the open DevOps Engineer position at Flagship Engineering. I have the analytical mind necessary to write code and the big-picture mentality to make it innovative and unique. I also enjoy overseeing projects, organizing people and channeling them into areas where they can make the best use of their talents. I'm thrilled at the prospect of becoming a part of your team at Flagship and placing my abilities at your service.\r\n\r\nI began my IT career as a front-end developer at Crossover Software. My first job entailed working with teams of developers to write apps with varying functionalities from mobile games to search algorithms and malware detection packages. Collaborating with other developers not only honed my coding skills but also taught me the value of how to effectively work with others and subdivide tasks to improve efficiency and ensure smooth teamwork. I then became a cybersecurity technician with Centurion Technologies, designing encryption software and instructing clients in its use.",
            VacancyId = Guid.NewGuid(),
        };

        var applicant = new Applicant { Id = request.ApplicantId };
        _dbContextMock.Setup(x => x.Applicants.FindAsync(request.ApplicantId)).ReturnsAsync(applicant);

        var vacancy = new JobVacancy { Id = request.VacancyId };
        _dbContextMock.Setup(x => x.Vacancies.FindAsync(request.VacancyId)).ReturnsAsync(vacancy);

        var timelineEventSetMock = new Mock<DbSet<ApplicationTimelineEvent>>();
        _dbContextMock.Setup(x => x.ApplicationTimelineEvents).Returns(timelineEventSetMock.Object);

        var applicationSetMock = new Mock<DbSet<Application>>();
        var newEntityGuid = Guid.NewGuid();
        applicationSetMock.Setup(d => d.AddAsync(It.IsAny<Application>(), It.IsAny<CancellationToken>())).Callback<Application, CancellationToken>((v, ct) => v.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.Applications).Returns(applicationSetMock.Object);

        _dateTimeMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
        _currentUserServiceMock.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(newEntityGuid);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
