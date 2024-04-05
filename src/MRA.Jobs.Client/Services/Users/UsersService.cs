using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Users;
using MudBlazor;

namespace MRA.Jobs.Client.Services.Users;

public class UsersService(
    NavigationManager navigationManager,
    IHttpClientService httpClientService,
    IConfiguration configuration,
    ISnackbar snackbar) : IUsersService
{
    public async Task<PagedList<UserResponse>> GetPagedListUsers(GetPagedListUsersQuery query, string url = null)
    {
        var queryParam = HttpUtility.ParseQueryString(string.Empty);
        if (!query.Skills.IsNullOrEmpty()) queryParam["Skills"] = query.Skills;
        queryParam["Page"] = query.Page.ToString();
        queryParam["PageSize"] = query.PageSize.ToString();
        if (!query.Filters.IsNullOrEmpty()) queryParam["Filters"] = query.Filters;

        var newUri = $"{navigationManager.BaseUri}{url}?{queryParam}";
        navigationManager.NavigateTo(newUri, forceLoad: false);

        var response =
            await httpClientService.GetFromJsonAsync<PagedList<UserResponse>>(
                configuration.GetJobsUrl($"user?{queryParam}"));
        if (!response.Success)
        {
            snackbar.ShowIfError(response, configuration["ServerIsNotResponding"]);
            return new PagedList<UserResponse>();
        }
        return response.Result;
    }

    public Task<List<UserResponse>> GetListUsers(GetListUsersQuery query)
    {
        throw new NotImplementedException();
    }
}