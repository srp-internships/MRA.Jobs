namespace MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;

public class AddNoteToApplicationCommandValidator : AbstractValidator<AddNoteToApplicationCommand>
{
    public AddNoteToApplicationCommandValidator()
    {
        RuleFor(v => v.Slug)
            .NotEmpty();
        RuleFor(v => v.Note)
            .NotEmpty()
            .MaximumLength(200);
    }
}