namespace MRA.Jobs.Application.Contracts.ContentService;

public interface IContentService
{
    string this[string name] { get; }
}