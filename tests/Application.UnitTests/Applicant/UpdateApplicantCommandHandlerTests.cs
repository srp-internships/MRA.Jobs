using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.UpdateApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;
using Domain.Entities;

public class UpdateApplicantCommandHandlerTests : BaseTestFixture
{
    private UpdateApplicantCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new UpdateApplicantCommandHandler(
            _dbContextMock.Object, Mapper);
    }

    [Test]
    public void Handle_GivenNonExistentApplicantId_ShouldThrowNotFoundException()
    {
        // Arrange 
        var command = new UpdateApplicantCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Applicants.FindAsync(command.Id))
            .ReturnsAsync(null as Applicant);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert 
        act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Applicant)}*{command.Id}*");
    }


}