
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MRA.Configurations.Common.Interfaces.Services;
using MRA.Configurations.OsonSms.SmsService;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using IEmailService = MRA.Configurations.Common.Interfaces.Services.IEmailService;

namespace MRA.Jobs.Application.UnitTests;

[TestFixture]
public abstract class BaseTestFixture
{
    protected Mock<IApplicationDbContext> _dbContextMock;
    public static IMapper Mapper { get; set; }
    protected Mock<IDateTime> _dateTimeMock;
    protected Mock<ICurrentUserService> _currentUserServiceMock;
    protected Mock<ISlugGeneratorService> _slugGenerator;
    protected Mock<IEmailService> _emailServiceMock;
    protected Mock<IHtmlService> _htmlServiceMock;
    protected Mock<ICvService> _cvService;
    protected Mock<IVacancyTaskService> _vacancyTaskService;
    protected Mock<IidentityService> _identityService;
    protected Mock<ISmsService> _smsServiceMock;
    protected Mock<ILogger<SmsService>> _loggerMock;
    protected Mock<IConfiguration> _configurationMock;

    [SetUp]
    public virtual void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _identityService = new Mock<IidentityService>();
        _cvService = new Mock<ICvService>();
        _dateTimeMock = new Mock<IDateTime>();
        _slugGenerator = new Mock<ISlugGeneratorService>();
        _emailServiceMock = new Mock<IEmailService>();
        _htmlServiceMock = new Mock<IHtmlService>();
        _dateTimeMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _currentUserServiceMock.Setup(r => r.GetUserId()).Returns(Guid.Empty);
        _smsServiceMock = new Mock<ISmsService>();
        _loggerMock = new Mock<ILogger<SmsService>>();
        _configurationMock = new Mock<IConfiguration>(); 
    }
}
