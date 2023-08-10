using MRA.JobsTemp.Application.Common.Mappings;
using MRA.JobsTemp.Domain.Entities;

namespace MRA.JobsTemp.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
