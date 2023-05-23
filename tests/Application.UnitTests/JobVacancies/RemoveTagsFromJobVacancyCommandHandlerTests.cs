namespace MRA.Jobs.Application.UnitTests.JobVacancy;

using System.Collections.ObjectModel;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;
using MRA.Jobs.Domain.Entities;
public class RemoveTagsFromJobVacancyCommandHandlerTests : BaseTestFixture
{
    private RemoveTagsFromJobVacancyCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new RemoveTagsFromJobVacancyCommandHandler(_dbContextMock.Object,
            Mapper,
            _currentUserServiceMock.Object, _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_RemovesTagsFromJobVacancy_ReturnsTrue()
    {
        // Arrange
        var JobVacancyDbSetMock = new Mock<DbSet<JobVacancy>>();
        var vacancyTagsDbSetMock = new Mock<DbSet<VacancyTag>>();
        var timelimeEvenDbSetMock = new Mock<DbSet<VacancyTimelineEvent>>();

        _dbContextMock.Setup(x => x.JobVacancies).Returns(JobVacancyDbSetMock.Object);
        _dbContextMock.Setup(x => x.VacancyTags).Returns(vacancyTagsDbSetMock.Object);
        _dbContextMock.Setup(x => x.VacancyTimelineEvents).Returns(timelimeEvenDbSetMock.Object);

        var JobVacancyId = Guid.NewGuid();

        var JobVacancy = new JobVacancy
        {
            Id = JobVacancyId,
        };

        var tag1 = new Tag
        {
            Id = Guid.NewGuid(),
            Name = "Tag1"
        };
        var tag2 = new Tag
        {
            Id = Guid.NewGuid(),
            Name = "Tag2"
        };

        var vacancyTag1 = new VacancyTag
        {
            VacancyId = JobVacancyId,
            TagId = tag1.Id,
            Tag = tag1
        };
        var vacancyTag2 = new VacancyTag
        {
            VacancyId = JobVacancyId,
            TagId = tag2.Id,
            Tag = tag2
        };
        JobVacancy.Tags = new Collection<VacancyTag>() { vacancyTag1, vacancyTag2 };

        JobVacancyDbSetMock.Setup(x => x.FindAsync(new object[] { JobVacancyId }, CancellationToken.None))
            .ReturnsAsync(JobVacancy);

        var command = new RemoveTagsFromJobVacancyCommand
        {
            JobVacancyId = JobVacancyId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        vacancyTagsDbSetMock.Verify(x => x.Remove(It.Is<VacancyTag>(ut => ut.VacancyId == JobVacancyId && ut.TagId == tag1.Id)), Times.Once);
        vacancyTagsDbSetMock.Verify(x => x.Remove(It.Is<VacancyTag>(ut => ut.VacancyId == JobVacancyId && ut.TagId == tag2.Id)), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        Assert.IsTrue(result);
    }
}


