namespace MRA.Jobs.Application.UnitTests;

[TestFixture]
public abstract class BaseTestFixture
{
    protected Mock<IApplicationDbContext> _dbContextMock;
    public static IMapper Mapper { get; set; }
    protected Mock<IDateTime> _dateTimeMock;
    protected Mock<ICurrentUserService> _currentUserServiceMock;

    [SetUp]
    public virtual void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _dateTimeMock = new Mock<IDateTime>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
    }
}
