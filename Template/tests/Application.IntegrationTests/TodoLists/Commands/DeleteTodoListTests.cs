using MRA.JobsTemp.Application.Common.Exceptions;
using MRA.JobsTemp.Application.TodoLists.Commands.CreateTodoList;
using MRA.JobsTemp.Application.TodoLists.Commands.DeleteTodoList;
using MRA.JobsTemp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace MRA.JobsTemp.Application.IntegrationTests.TodoLists.Commands;

using static Testing;

public class DeleteTodoListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteTodoListCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoList()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await SendAsync(new DeleteTodoListCommand(listId));

        var list = await FindAsync<TodoList>(listId);

        list.Should().BeNull();
    }
}
