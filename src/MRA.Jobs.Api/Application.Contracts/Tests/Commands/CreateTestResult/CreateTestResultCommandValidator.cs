namespace MRA.Jobs.Application.Contracts.Tests.Commands.CreateTestResult;

public class CreateTestResultCommandValidator : AbstractValidator<CreateTestResultCommand>
{
    public CreateTestResultCommandValidator()
    {
        RuleFor(x => x.TestId).NotEmpty();
        RuleFor(x => x.Slug).NotEmpty();
        RuleFor(x => x.Score).NotEmpty();
    }
}