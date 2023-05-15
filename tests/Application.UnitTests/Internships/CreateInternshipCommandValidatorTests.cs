using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Features.Internships.Command.CreateInternship;

namespace MRA.Jobs.Application.UnitTests.Internships;

[TestFixture]
public class CreateInternshipCommandValidatorTests
{
    private CreateInternshipCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateInternshipCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        //Arrange
        var request = new CreateInternshipCommand();

        //Act
        var result = _validator.TestValidate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_TitleEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { Title = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { ShortDescription = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { Description = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validate_PublishDateEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { PublishDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    public void Validate_EndDateEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { EndDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { CategoryId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_ApplicationDeadlineEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { ApplicationDeadline = DateTime.MinValue };

        // Act 
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ApplicationDeadline);
    }

    [Test]
    public void Validate_DurationEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { Duration = 0 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public void Validate_StipendEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand { Stipend = 0 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Stipend);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        var request = new CreateInternshipCommand
        {
            Title = "Test Internship",
            ShortDescription = "Test Internship Short Description",
            Description = "Test Internship Description",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            CategoryId = Guid.NewGuid(),
            ApplicationDeadline = DateTime.Now.AddDays(60),
            Duration = 10,
            Stipend = 1
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
