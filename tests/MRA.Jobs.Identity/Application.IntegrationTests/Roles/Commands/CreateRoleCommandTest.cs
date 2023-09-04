﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Azure;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;
using MRA.Identity.Domain.Entities;
using NUnit.Framework.Constraints;

namespace MRA.Jobs.Application.IntegrationTests.Roles.Commands;
public class CreateRoleCommandTest : BaseTest
{
    [Test]
    public async Task CreateRoleCommand_ShouldCreateRoleCommand_Success()
    {
        var command = new CreateRoleCommand
        {
            RoleName = "Test",
        };

        var response = await _client.PostAsJsonAsync("/api/roles", command);

        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task CreateRoleCommand_ShouldCreateRoleCommand_Faild()
    {
        var role = new ApplicationRole
        {
            Name = "Test2",
            Slug = "test2"
        };

        await AddEntity(role);


        var command = new CreateRoleCommand
        {
            RoleName = "Test2",
        };

        var response = await _client.PostAsJsonAsync("/api/roles", command);
        response = await _client.PostAsJsonAsync("/api/roles", command);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }
}