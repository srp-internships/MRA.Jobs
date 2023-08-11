using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.UpdateJobVacancy;

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
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { Id = Guid.Empty };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_TitleIsRequired()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { Title = null };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionIsRequired()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { ShortDescription = null };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionIsRequired()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { Description = null };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validate_PublishDateIsRequired()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    public void Validate_EndDateIsRequired()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdIsRequired()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { CategoryId = Guid.Empty };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_RequiredYearOfExperienceCannotBeNegative()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { RequiredYearOfExperience = -1 };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RequiredYearOfExperience);
    }

    [Test]
    public void Validate_WorkScheduleMustBeValidEnumValue()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand { WorkSchedule = (WorkSchedule)10 };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.WorkSchedule);
    }

    [Test]
    public void Validate_GivenValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        UpdateJobVacancyCommand command = new UpdateJobVacancyCommand
        {
            Id = Guid.NewGuid(),
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 0,
            WorkSchedule = WorkSchedule.FullTime
        };

        // Act
        TestValidationResult<UpdateJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}