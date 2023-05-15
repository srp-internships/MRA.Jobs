using MediatR;

namespace MRA.Jobs.Application.Contracts.Tag.Commands;
public class UpdateTagCommand :IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
