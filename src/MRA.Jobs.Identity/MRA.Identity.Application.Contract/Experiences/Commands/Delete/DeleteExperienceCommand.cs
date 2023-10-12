using MediatR;

namespace MRA.Identity.Application.Contract.Experience.Command.Delete;

public class DeleteExperienceCommand: IRequest<ApplicationResponse<bool>>
{
    public Guid Id { get; set; }
}