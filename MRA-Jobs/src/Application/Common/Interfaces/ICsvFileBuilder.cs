using MRA_Jobs.Application.TodoLists.Queries.ExportTodos;

namespace MRA_Jobs.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}