using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.CreateVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class CreateVacancyCategoryCommandHandlerTests : BaseTestFixture
{
    private CreateVacancyCategoryCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateVacancyCategoryCommandHandler(
            _slugGenerator.Object,
            _dbContextMock.Object,
            Mapper);
    }

    [Test]
    [Ignore("must be Integration test ")]
    public async Task Handle_ValidRequest_ShouldCreateVacancyCategory()
    {
        // Arrange
        var request = new CreateVacancyCategoryCommand
        {
            Name = "Software Development"
        };

        var categorySetMock = new Mock<DbSet<VacancyCategory>>();
        var newEntityGuid = Guid.NewGuid();
        var slug = "software-development";
        categorySetMock.Setup(d => d.AddAsync(
            It.IsAny<VacancyCategory>(),
            It.IsAny<CancellationToken>())
        ).Callback<VacancyCategory,
            CancellationToken>((v, ct) => v.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.Categories).
            Returns(categorySetMock.Object);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(slug);

        categorySetMock.Verify(x => x.AddAsync(It.Is<VacancyCategory>(vc =>
            vc.Name == request.Name
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

