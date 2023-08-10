using System.Globalization;
using MRA.JobsTemp.Application.Common.Interfaces;
using MRA.JobsTemp.Application.TodoLists.Queries.ExportTodos;
using MRA.JobsTemp.Infrastructure.Files.Maps;
using CsvHelper;

namespace MRA.JobsTemp.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
