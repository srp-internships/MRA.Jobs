using MRA.Jobs.Application.Features.VacancyCategories.Command.DeleteVacancyCategory;
using DeleteVacancyCategoryCommand = MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory.DeleteVacancyCategoryCommand;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;
public class DeleteVacancyCategoryCommandHandlerTests : BaseTestFixture
{
    private DeleteVacancyCategoryCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteVacancyCategoryCommandHandler(_dbContextMock.Object);
    }

    [Test]
    [Ignore("slug")]
    public async Task Handle_CategoryVacancyExists_ShouldRemoveCategoryVacancy()
    {
        //Arrange
        var vacancyCategory = new VacancyCategory { Slug=string.Empty };
        _dbContextMock.Setup(x => x.Categories.FindAsync(
            new object[] { vacancyCategory.Id },
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyCategory);

        var command = new DeleteVacancyCategoryCommand { Slug = vacancyCategory.Slug };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.Categories.Remove(vacancyCategory), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.That(result);
    }

    [Test]
    [Ignore("Slug")]
    public void Handle_VacancyCategoryNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteVacancyCategoryCommand { Slug=string.Empty };


        _dbContextMock.Setup(x => x.Categories
            .FindAsync(new object[] { command.Slug }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as VacancyCategory);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.Categories.Remove(It.IsAny<VacancyCategory>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}
