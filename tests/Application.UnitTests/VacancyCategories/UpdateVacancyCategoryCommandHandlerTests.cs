using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.UpdateVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class UpdateVacancyCategoryCommandHandlerTests : BaseTestFixture
{
    private UpdateVacancyCategoryCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _handler = new UpdateVacancyCategoryCommandHandler(
            _dbContextMock.Object, Mapper);
    }

    [Test]
    public void Handle_GivenNonExistentVacancyCategoryId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateVacancyCategoryCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x =>x.Categories.FindAsync(command.Id)).ReturnsAsync(null as VacancyCategory);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);


        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(VacancyCategory)}*{command.Id}*");
    }
    public async Task Handle_GivenValidCommand_ShouldUpdateVacancyCategory()
    {
        // Arrange
        var command = new UpdateVacancyCategoryCommand
        {
            Id = Guid.NewGuid(),
            Name = "New Category Title",
        };

        var existingVacancyCategory = new VacancyCategory { Id = command.Id };
        _dbContextMock.Setup(x =>x.Categories.FindAsync(command.Id))
            .ReturnsAsync(existingVacancyCategory);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(command.Id);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        existingVacancyCategory.Name.Should().Be(command.Name);
    }
}
