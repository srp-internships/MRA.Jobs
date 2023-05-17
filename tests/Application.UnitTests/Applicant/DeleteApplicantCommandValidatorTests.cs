using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.DeleteApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;

[TestFixture]
public class DeleteApplicantCommandValidatorTests
{
    private DeleteApplicantCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteApplicantCommandValidator();
    }


    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = new DeleteApplicantCommand { Id = Guid.Empty };
        
        // Act 
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange 
        var command = new DeleteApplicantCommand { Id = Guid.NewGuid() };
        
        // Act 
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}