using MediatR;

namespace MRA.Identity.Application.Contract.Experience.Command.Delete;

public class DeleteExperienceCommand: IRequest<bool>
{
    public Guid Id { get; set; }
}