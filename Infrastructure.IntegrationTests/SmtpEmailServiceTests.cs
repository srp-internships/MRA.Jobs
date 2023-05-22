using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MRA.Jobs.Application.Common.Models;
using MRA.Jobs.Infrastructure.Services;
using netDumbster.smtp;

namespace Infrastructure.IntegrationTests;

[TestFixture]
public class SmtpEmailServiceTests
{
    private SmtpEmailService _emailService;
    private Mock<IOptions<SmtpSettings>> _smtpSettingsMock;
    private Mock<ILogger<SmtpEmailService>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _smtpSettingsMock = new Mock<IOptions<SmtpSettings>>();
        _loggerMock = new Mock<ILogger<SmtpEmailService>>();

        _smtpSettingsMock.Setup(s => s.Value).Returns(new SmtpSettings
        {
            Server = "localhost",
            Port = 9009,
            Username = "vernice77@ethereal.email",
            Password = "Jr2VSHPbDEjEgwZmcF",
            EnableSsl = true
        });

        _emailService = new SmtpEmailService(_smtpSettingsMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task SendEmailAsync_ValidMessage_EmailSentSuccessfully()
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            From = "from@test.com",
            To = "vernice77@ethereal.email",
            Subject = "Test Subject",
            Body = "Test Body",
            IsHtml = true
        };

        var server = SimpleSmtpServer.Start(9009);

        // Act
        await _emailService.SendEmailAsync(emailMessage);

        // Assert

        var emails = server.ReceivedEmail;
        Assert.AreEqual(1, emails.Count());
    }
}
