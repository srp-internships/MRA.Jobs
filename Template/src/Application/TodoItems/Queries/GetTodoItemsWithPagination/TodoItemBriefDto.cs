using MRA.JobsTemp.Application.Common.Mappings;
using MRA.JobsTemp.Domain.Entities;

namespace MRA.JobsTemp.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class TodoItemBriefDto : IMapFrom<TodoItem>
{
    public int Id { get; set; }

    public int ListId { get; set; }

    public string? Title { get; set; }

    public bool Done { get; set; }
}
