using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancy;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

[TestFixture]
public class CreateJobVacancyCommandValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new CreateJobVacancyCommandValidator();
    }

    private CreateJobVacancyCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand();

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_TitleEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { Title = "" };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { ShortDescription = "" };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { Description = "" };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    [Ignore("update")]
    public void Validate_PublishDateEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    [Ignore("update")]
    public void Validate_EndDateEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { CategoryId = Guid.Empty };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_RequiredYearOfExperienceNegative_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { RequiredYearOfExperience = -1 };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RequiredYearOfExperience);
    }

    [Test]
    public void Validate_RequiredYearOfExperienceZero_ShouldPass()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { RequiredYearOfExperience = 0 };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.RequiredYearOfExperience);
    }

    [Test]
    public void Validate_WorkScheduleInvalid_ShouldFailValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand { WorkSchedule = (Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule)3 };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.WorkSchedule);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand
        {
            Title = "Test Job Vacancy",
            ShortDescription = "Test Job Vacancy Short Description",
            Description = "Test Job Vacancy Description",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 0,
            WorkSchedule = Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule.FullTime
        };

        // Act
        TestValidationResult<CreateJobVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}