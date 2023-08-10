﻿using MRA.Jobs.Application.Contracts.Tests.Commands;

namespace MRA.Jobs.Application.Features.Tests.Commands.CreateTestResult;
public class CreateTestResultCommandValidator : AbstractValidator<CreateTestResultCommand>
{
    public CreateTestResultCommandValidator()
    {
        RuleFor(x => x.TestId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Score).NotEmpty();
    }
}
