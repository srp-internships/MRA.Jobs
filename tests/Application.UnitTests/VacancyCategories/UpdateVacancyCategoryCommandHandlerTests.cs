using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.UpdateVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class UpdateVacancyCategoryCommandHandlerTests : BaseTestFixture
{
    private UpdateVacancyCategoryCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new UpdateVacancyCategoryCommandHandler(
            _dbContextMock.Object, Mapper);
    }

    [Test]
    public void Handle_GivenNonExistentVacancyCategoryId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateVacancyCategoryCommand { Slug = string.Empty };
        _dbContextMock.Setup(x => x.Categories.FindAsync(command.Slug)).ReturnsAsync(null as VacancyCategory);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);


        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(VacancyCategory)}*{command.Slug}*");
    }

    [Test]
    [Ignore("slug")]
    public async Task Handle_GivenValidCommand_ShouldUpdateVacancyCategory()
    {
        // Arrange
        var command = new UpdateVacancyCategoryCommand
        {
            Slug = string.Empty,
            Name = "New Category Title",
        };

        var categoryDbSetMock = new Mock<DbSet<VacancyCategory>>();

        _dbContextMock.Setup(x => x.Categories).Returns(categoryDbSetMock.Object);

        var existingVacancyCategory = new VacancyCategory { Slug = command.Slug, Name = command.Name };

        categoryDbSetMock.Setup(x => x.FindAsync(new object[] { command.Slug }, CancellationToken.None))
            .ReturnsAsync(existingVacancyCategory);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(command.Slug);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        existingVacancyCategory.Name.Should().Be(command.Name);
    }
}
