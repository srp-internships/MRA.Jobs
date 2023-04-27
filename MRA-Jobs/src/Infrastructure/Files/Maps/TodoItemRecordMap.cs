using System.Globalization;
using MRA_Jobs.Application.TodoLists.Queries.ExportTodos;
using CsvHelper.Configuration;

namespace MRA_Jobs.Infrastructure.Files.Maps;

public class TodoItemRecordMap : ClassMap<TodoItemRecord>
{
    public TodoItemRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        Map(m => m.Done).Convert(c => c.Value.Done ? "Yes" : "No");
    }
}