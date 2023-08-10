using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.CreateVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class CreateVacancyCategoryCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateVacancyCategoryCommandHandler(
            _dbContextMock.Object,
            Mapper);
    }

    private CreateVacancyCategoryCommandHandler _handler;

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateVacancyCategory()
    {
        // Arrange
        CreateVacancyCategoryCommand request = new CreateVacancyCategoryCommand { Name = "Software Development" };

        Mock<DbSet<VacancyCategory>> categorySetMock = new Mock<DbSet<VacancyCategory>>();
        Guid newEntityGuid = Guid.NewGuid();
        categorySetMock.Setup(d => d.AddAsync(
            It.IsAny<VacancyCategory>(),
            It.IsAny<CancellationToken>())
        ).Callback<VacancyCategory,
            CancellationToken>((v, ct) => v.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.Categories).Returns(categorySetMock.Object);

        // Act
        Guid result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(newEntityGuid);

        categorySetMock.Verify(x => x.AddAsync(It.Is<VacancyCategory>(vc =>
            vc.Name == request.Name
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}