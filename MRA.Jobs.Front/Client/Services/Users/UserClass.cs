using System.Collections.Generic;
using System.Net.Http.Json;

namespace MRA.Jobs.Front.Client.Services.Users;

public class UserClass
{
    private readonly HttpClient _httpClient;

    public UserClass(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async void GetProduct()
    {
        await _httpClient.GetFromJsonAsync<List<UserClass>>("api/");
    }
}
