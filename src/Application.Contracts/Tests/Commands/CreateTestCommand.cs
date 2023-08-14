using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Contracts.Tests.Commands;
public class CreateTestCommand : IRequest<TestInfoDTO>
{
    public Guid Id { get; set; }
    public string Slug { get; set; }
    public long NumberOfQuestion { get; set; }
    public List<string> Categories { get; set; }
}