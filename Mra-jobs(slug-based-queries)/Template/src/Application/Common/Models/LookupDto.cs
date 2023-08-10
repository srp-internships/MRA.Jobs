using MRA.JobsTemp.Application.Common.Mappings;
using MRA.JobsTemp.Domain.Entities;

namespace MRA.JobsTemp.Application.Common.Models;

// Note: This is currently just used to demonstrate applying multiple IMapFrom attributes.
public class LookupDto : IMapFrom<TodoList>, IMapFrom<TodoItem>
{
    public int Id { get; set; }

    public string? Title { get; set; }
}
