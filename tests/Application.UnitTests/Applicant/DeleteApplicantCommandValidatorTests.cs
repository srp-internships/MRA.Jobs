using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicants.Command.DeleteApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;

[TestFixture]
public class DeleteApplicantCommandValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteApplicantCommandValidator();
    }

    private DeleteApplicantCommandValidator _validator;


    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        DeleteApplicantCommand command = new DeleteApplicantCommand { Id = Guid.Empty };

        // Act 
        TestValidationResult<DeleteApplicantCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange 
        DeleteApplicantCommand command = new DeleteApplicantCommand { Id = Guid.NewGuid() };

        // Act 
        TestValidationResult<DeleteApplicantCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}