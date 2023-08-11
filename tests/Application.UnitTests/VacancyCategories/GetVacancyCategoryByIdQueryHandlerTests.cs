using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class GetVacancyCategoryByIdQueryHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetVacancyCategoryByIdQueryHandler(_dbContextMock.Object, Mapper);
    }

    private GetVacancyCategoryByIdQueryHandler _handler;

    [Test]
    public async Task Handle_GivenValidQuery_ShouldReturnVacancyCategoryDetailsDTO()
    {
        GetVacancyCategoryByIdQuery query = new GetVacancyCategoryByIdQuery { Id = Guid.NewGuid() };

        VacancyCategory VacancyCategory = new VacancyCategory { Id = query.Id, Name = "Software Development" };
        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(VacancyCategory);

        // Act
        CategoryResponse result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(VacancyCategory.Id);
        result.Name.Should().Be(VacancyCategory.Name);
    }

    [Test]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        GetVacancyCategoryByIdQuery query = new GetVacancyCategoryByIdQuery { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VacancyCategory)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}