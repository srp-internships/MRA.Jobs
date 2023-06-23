using Microsoft.JSInterop;

namespace MRA.Jobs.Client.Services.LocalStorageService;

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jSRuntime;

    public LocalStorageService(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    public async Task<string[]> GetItemArrayAsync(string key)
    {
        var data = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);
        if (!string.IsNullOrEmpty(data))
            return data.Split("\0");
        return null;
    }

    public async Task<string> GetItemAsync(string key)
    {
        return await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task RemoveAsync(string key)
    {
        await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task SaveItemArrayAsync(string key, string[] values)
    {
       if(values != null)
            await _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, string.Join("\0", values));
    }

    public async Task SaveItemAsync(string key, string value)
    {
        await _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }
}
