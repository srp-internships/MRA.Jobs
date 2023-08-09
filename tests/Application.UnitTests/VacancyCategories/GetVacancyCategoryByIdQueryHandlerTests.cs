using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class GetVacancyCategoryBySlugQueryHandlerTests : BaseTestFixture
{
    private GetVacancyCategoryBySlugQueryHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetVacancyCategoryBySlugQueryHandler(_dbContextMock.Object, Mapper);
    }

    [Test]
    public async Task Handle_GivenValidQuery_ShouldReturnVacancyCategoryDetailsDTO()
    {
        var query = new GetVacancyCategoryBySlugQuery { Slug= "categories-softwaredevelopment" };

        var VacancyCategory = new VacancyCategory
        {
            Id = Guid.NewGuid(),
            Name = "Software Development",
        };
        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { query.Slug }, It.IsAny<CancellationToken>())).ReturnsAsync(VacancyCategory);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(VacancyCategory.Id);
        result.Name.Should().Be(VacancyCategory.Name);
 
    }

    [Test]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetVacancyCategoryBySlugQuery {Slug="fsfsefse"};

        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { query.Slug }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VacancyCategory)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}