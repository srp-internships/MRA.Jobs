using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class UpdateReviewerCommandValidatorTests
{
    private UpdateReviewerCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateReviewerCommandValidator();
    }

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange 
        UpdateReviewerCommand command = new UpdateReviewerCommand { Id = Guid.Empty };

        // Act 
        TestValidationResult<UpdateReviewerCommand> result = _validator.TestValidate(command);

        // Assert 
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_FirstNameIsRequired()
    {
        // Arrange
        UpdateReviewerCommand command = new UpdateReviewerCommand { FirstName = null };

        // Act
        TestValidationResult<UpdateReviewerCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public void Validate_LastNameIsRequired()
    {
        // Arrange
        UpdateReviewerCommand command = new UpdateReviewerCommand { LastName = null };

        // Act
        TestValidationResult<UpdateReviewerCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public void Validate_AvatarIsRequired()
    {
        // Arrange
        UpdateReviewerCommand command = new UpdateReviewerCommand { Avatar = null };

        // Act
        TestValidationResult<UpdateReviewerCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Avatar);
    }

    [Test]
    public void Validate_PhoneNumberIsRequired()
    {
        // Arrange
        UpdateReviewerCommand command = new UpdateReviewerCommand { PhoneNumber = null };

        // Act
        TestValidationResult<UpdateReviewerCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public void Validate_PatronymicNumberIsRequired()
    {
        // Arrange
        UpdateReviewerCommand command = new UpdateReviewerCommand { Patronymic = null };

        // Act
        TestValidationResult<UpdateReviewerCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Patronymic);
    }

    [Test]
    public void Validate_EmailNumberIsRequired()
    {
        // Arrange
        UpdateReviewerCommand command = new UpdateReviewerCommand { Email = null };

        // Act
        TestValidationResult<UpdateReviewerCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}