using MRA_Jobs.Application.Common.Exceptions;
using MRA_Jobs.Application.TodoLists.Commands.CreateTodoList;
using MRA_Jobs.Application.TodoLists.Commands.DeleteTodoList;
using MRA_Jobs.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace MRA_Jobs.Application.IntegrationTests.TodoLists.Commands;

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