using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MRA.Jobs.Application.Common.Models;
using MRA.Jobs.Infrastructure.Services;

namespace Infrastructure.UnitTests.Services;

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
            Server = "smtp.server.com",
            Port = 587,
            Username = "username",
            Password = "password",
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
            To = "to@test.com",
            Subject = "Test Subject",
            Body = "Test Body",
            IsHtml = true
        };

        // Act
        await _emailService.SendEmailAsync(emailMessage);

        // Assert

    }

    private void SetPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(obj, value);
    }
}
