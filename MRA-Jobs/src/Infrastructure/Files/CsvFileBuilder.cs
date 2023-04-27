using System.Globalization;
using MRA_Jobs.Application.Common.Interfaces;
using MRA_Jobs.Application.TodoLists.Queries.ExportTodos;
using MRA_Jobs.Infrastructure.Files.Maps;
using CsvHelper;

namespace MRA_Jobs.Infrastructure.Files;

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