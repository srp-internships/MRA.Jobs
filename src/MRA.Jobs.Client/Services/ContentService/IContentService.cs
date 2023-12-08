namespace MRA.Jobs.Client.Services.ContentService;

public interface IContentService
{
    string this[string name] { get; }
    public Task ChangeCulture(string name);
    public Task InitializeCultureAsync();
}