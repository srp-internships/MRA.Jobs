using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class GetVacancyCategoryByIdQueryHandlerTests : BaseTestFixture
{
    private GetVacancyCategoryByIdQueryHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetVacancyCategoryByIdQueryHandler(_dbContextMock.Object, Mapper);
    }

    [Test]
    public async Task Handle_GivenValidQuery_ShouldReturnVacancyCategoryDTO()
    {
        var query = new GetByIdVacancyCategoryQuery { Id = Guid.NewGuid() };

        var VacancyCategory = new VacancyCategory
        {
            Id = query.Id,
            Name = "Software Development",
        };
        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>())).ReturnsAsync(VacancyCategory);

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
        var query = new GetByIdVacancyCategoryQuery { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VacancyCategory)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}