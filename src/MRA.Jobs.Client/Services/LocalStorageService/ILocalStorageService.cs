namespace MRA.Jobs.Client.Services.LocalStorageService;

public interface ILocalStorageService
{
    public Task RemoveAsync(string key);
    public Task SaveItemAsync(string key, string value);
    public Task<string> GetItemAsync(string key);
    public Task SaveItemArrayAsync(string key, string[] values);
    public Task<string[]> GetItemArrayAsync(string key);
}
