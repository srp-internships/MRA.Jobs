using MediatR;

namespace MRA.Jobs.Application.Contracts.Tag.Commands;
public class CreateTagCommand : IRequest<Guid>
{
    public string Name { get; set; }

}
