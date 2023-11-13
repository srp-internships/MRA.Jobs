﻿using System.Net;
using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;
using MRA.Configurations.Services;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;

namespace MRA.Jobs.Application.IntegrationTests;

[TestFixture]
public class EmailTest : BaseTest
{
    [Test]
    public async Task Email_VerifyEmail_True()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test1@example.com",
            Password = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex221",
            LastName = "Makedonsky1",
            PhoneNumber = "+992223456789"
        };

        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        var splitted = SendEmailData.Body.Split("'")[1].Split("token=")[1];


        var responseEmail = await _client.GetAsync($"api/Auth/verify?token={splitted}");

        var stringres = responseEmail.Content.ReadAsStringAsync();

        // Assert

        responseEmail.EnsureSuccessStatusCode();
        var user = await GetEntity<ApplicationUser>(s => s.Email == request.Email);
        Assert.IsTrue(user.EmailConfirmed);
    }

    [Test]
    public async Task Email_VerifyEmail_BadRequest()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "test1@example.com",
            Password = "password@#12P",
            FirstName = "Alex",
            Username = "@Alex221",
            LastName = "Makedonsky1",
            PhoneNumber = "+992323456789"
        };
        // Act
        var response = await _client.PostAsJsonAsync("api/Auth/register", request);
        var userId = (await response.Content.ReadAsStringAsync()).Replace("\"", "");
        var splitted = "aaaa1";

        var responseEmail =
            await _client.GetAsync($"api/Auth/verify?token={WebUtility.UrlEncode(splitted)}&userId={userId}");
        // Assert

        Assert.That(responseEmail.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var user = await GetEntity<ApplicationUser>(s => s.Email == request.Email);
        Assert.IsFalse(user.EmailConfirmed);
    }
}