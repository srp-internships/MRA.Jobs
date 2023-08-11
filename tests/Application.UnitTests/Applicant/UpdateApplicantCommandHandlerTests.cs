using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicants.Command.UpdateApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;

public class UpdateApplicantCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new UpdateApplicantCommandHandler(
            _dbContextMock.Object, Mapper);
    }

    private UpdateApplicantCommandHandler _handler;

    [Test]
    public void Handle_GivenNonExistentApplicantId_ShouldThrowNotFoundException()
    {
        // Arrange 
        UpdateApplicantCommand command = new UpdateApplicantCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Applicants.FindAsync(command.Id))
            .ReturnsAsync(null as Domain.Entities.Applicant);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert 
        act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Domain.Entities.Applicant)}*{command.Id}*");
    }
}