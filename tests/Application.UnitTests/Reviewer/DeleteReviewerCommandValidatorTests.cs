using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.DeleteReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class DeleteReviewerCommandValidatorTests
{
    private DeleteReviewerCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteReviewerCommandValidator();
    }


    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        DeleteReviewerCommand command = new DeleteReviewerCommand { Id = Guid.Empty };

        // Act 
        TestValidationResult<DeleteReviewerCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange 
        DeleteReviewerCommand command = new DeleteReviewerCommand { Id = Guid.NewGuid() };

        // Act 
        TestValidationResult<DeleteReviewerCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}