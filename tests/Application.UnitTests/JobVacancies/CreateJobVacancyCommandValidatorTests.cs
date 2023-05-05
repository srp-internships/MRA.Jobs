using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancy;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

[TestFixture]
public class CreateJobVacancyCommandValidatorTests
{
    private CreateJobVacancyCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateJobVacancyCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_TitleEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { Title = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { ShortDescription = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { Description = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validate_PublishDateEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    public void Validate_EndDateEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { CategoryId = 0 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_RequiredYearOfExperienceNegative_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { RequiredYearOfExperience = -1 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RequiredYearOfExperience);
    }

    [Test]
    public void Validate_RequiredYearOfExperienceZero_ShouldPass()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { RequiredYearOfExperience = 0 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.RequiredYearOfExperience);
    }

    [Test]
    public void Validate_WorkScheduleInvalid_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand { WorkSchedule = (WorkSchedule)3 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.WorkSchedule);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        var request = new CreateJobVacancyCommand
        {
            Title = "Test Job Vacancy",
            ShortDescription = "Test Job Vacancy Short Description",
            Description = "Test Job Vacancy Description",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            CategoryId = 1,
            RequiredYearOfExperience = 0,
            WorkSchedule = WorkSchedule.FullTime
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
