using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.DeleteVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class DeleteVacancyCategoryCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteVacancyCategoryCommandHandler(_dbContextMock.Object);
    }

    private DeleteVacancyCategoryCommandHandler _handler;

    [Test]
    public async Task Handle_CategoryVacancyExists_ShouldRemoveCategoryVacancy()
    {
        //Arrange
        VacancyCategory vacancyCategory = new VacancyCategory { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Categories.FindAsync(
                new object[] { vacancyCategory.Id },
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyCategory);

        DeleteVacancyCategoryCommand command = new DeleteVacancyCategoryCommand { Id = vacancyCategory.Id };

        // Act
        bool result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.Categories.Remove(vacancyCategory), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_VacancyCategoryNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        DeleteVacancyCategoryCommand command = new DeleteVacancyCategoryCommand { Id = Guid.NewGuid() };


        _dbContextMock.Setup(x => x.Categories
                .FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as VacancyCategory);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.Categories.Remove(It.IsAny<VacancyCategory>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}