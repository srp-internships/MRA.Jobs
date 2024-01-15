using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Client;
using MudBlazor;

namespace MRA.Jobs.SSR.Client.Components.Pages.Admin;
public sealed partial class UserManager
{
    [Inject] private IdentityHttpClient Client { get; set; }
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
    
    private string _searchString = "";
  
    private IEnumerable<UserResponse> _pagedData;
    private MudTable<UserResponse> _table;

    private int _totalItems;
    
    
    private async Task<TableData<UserResponse>> ServerReload(TableState state)
    {
        await AuthStateProvider.GetAuthenticationStateAsync();
        IEnumerable<UserResponse> data = await Client.GetFromJsonAsync<List<UserResponse>>("user");
        await Task.Delay(100);
        data = data.Where(element =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (element.UserName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.FullName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }).ToArray();
        _totalItems = data.Count();
        switch (state.SortLabel)
        {
            case "UserName_field":
                data = data.OrderByDirection(state.SortDirection, o => o.UserName);
                break;
            case "FullName_field":
                data = data.OrderByDirection(state.SortDirection, o => o.FullName);
                break;
            case "EmailConfirmed_field":
                data = data.OrderByDirection(state.SortDirection, o => o.EmailConfirmed);
                break;
            case "Email_field":
                data = data.OrderByDirection(state.SortDirection, o => o.Email);
                break;
            case "PhoneNumberConfirmed_field":
                data = data.OrderByDirection(state.SortDirection, o => o.PhoneNumberConfirmed);
                break;
            case "PhoneNumber_field":
                data = data.OrderByDirection(state.SortDirection, o => o.PhoneNumber);
                break;
        }

        _pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new TableData<UserResponse>() {TotalItems = _totalItems, Items = _pagedData};
    }
    
    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }
}