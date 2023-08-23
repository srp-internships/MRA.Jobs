using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Common.SlugGeneratorService;

namespace MRA.Jobs.Application.UnitTests;

[TestFixture]
public abstract class BaseTestFixture
{
    protected Mock<IApplicationDbContext> _dbContextMock;
    public static IMapper Mapper { get; set; }
    protected Mock<IDateTime> _dateTimeMock;
    protected Mock<ICurrentUserService> _currentUserServiceMock;
    protected Mock<ISlugGeneratorService> _slugGenerator;
    HttpClient _httpClient;

    [SetUp]
    public virtual void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _dateTimeMock = new Mock<IDateTime>();
        _slugGenerator = new Mock<ISlugGeneratorService>();
        _dateTimeMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _currentUserServiceMock.Setup(r => r.GetId()).Returns(Guid.Empty);
    }
}
