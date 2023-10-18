using MediatR;

namespace MRA.Identity.Application.Contract.Educations.Command.Delete;

public class DeleteEducationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}