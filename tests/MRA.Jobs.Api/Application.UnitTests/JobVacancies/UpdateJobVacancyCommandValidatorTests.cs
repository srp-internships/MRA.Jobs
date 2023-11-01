using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;
using MRA.Jobs.Application.Features.JobVacancies.Commands.UpdateJobVacancy;
using UpdateJobVacancyCommandValidator = MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update.UpdateJobVacancyCommandValidator;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

public class UpdateJobVacancyCommandValidatorTests
{
    private UpdateJobVacancyCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateJobVacancyCommandValidator();
    }

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Test]
    public void Validate_TitleIsRequired()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { Title = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionIsRequired()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { ShortDescription = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionIsRequired()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { Description = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    [Ignore("update")]
    public void Validate_PublishDateIsRequired()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    [Ignore("update")]
    public void Validate_EndDateIsRequired()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdIsRequired()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { CategoryId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_RequiredYearOfExperienceCannotBeNegative()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { RequiredYearOfExperience = -1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RequiredYearOfExperience);
    }

    [Test]
    public void Validate_WorkScheduleMustBeValidEnumValue()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { WorkSchedule = (Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule)10 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.WorkSchedule);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_GivenValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand
        {
            Slug = string.Empty,
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 0,
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
