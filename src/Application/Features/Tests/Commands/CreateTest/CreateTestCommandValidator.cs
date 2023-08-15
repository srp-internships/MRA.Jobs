using MRA.Jobs.Application.Contracts.Tests.Commands;

namespace MRA.Jobs.Application.Features.Tests.Commands.CreateTest;

public class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
{
    public CreateTestCommandValidator()
    {
        RuleFor(x => x.NumberOfQuestion).NotEmpty();
        RuleFor(x => x.Categories).NotEmpty();
        RuleForEach(x => x.Categories).NotEmpty();
        RuleFor(x => x.Slug).NotEmpty();
    }
}