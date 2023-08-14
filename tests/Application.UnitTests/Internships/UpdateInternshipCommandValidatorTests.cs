using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Features.InternshipVacancies.Command.Update;

namespace MRA.Jobs.Application.UnitTests.Internships;
public class UpdateInternshipCommandValidatorTests
{
    private UpdateInternshipVacancyCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateInternshipVacancyCommandValidator();
    }

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Test]
    public void Validate_TitleIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { Title = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { ShortDescription = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { Description = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validate_PublishDateIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    public void Validate_EndDateIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { CategoryId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_ApplicationDeadlineIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { ApplicationDeadline = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ApplicationDeadline);
    }

    [Test]
    public void Validate_DurationIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { Duration = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public void Validate_StipendIsRequired()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { Stipend = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Stipend);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_GivenValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand
        {
            Slug=string.Empty,
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            ApplicationDeadline = new DateTime(2023, 05, 20),
            Duration = 10,
            Stipend = 1000
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
