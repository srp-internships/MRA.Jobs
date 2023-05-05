using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Features.JobVacancies.Commands.DeleteJobVacancy;
using MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

[TestFixture]
public class DeleteJobVacancyCommandValidatorTests
{
    private DeleteJobVacancyCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteJobVacancyCommandValidator();
    }

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = new DeleteJobVacancyCommand { Id = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var command = new DeleteJobVacancyCommand { Id = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}

public class GetJobVacancyByIdQueryValidatorTests
{
    private GetJobVacancyByIdQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetJobVacancyByIdQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        var query = new GetJobVacancyByIdQuery { Id = 0 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        var query = new GetJobVacancyByIdQuery { Id = -1 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}