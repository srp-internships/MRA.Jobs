using Sieve.Models;

namespace MRA.Jobs.Application.Contracts.Common;

public class PagedListQuery<T> : SieveModel, IRequest<PagedList<T>>
{
}

public class PagedList<T>
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPageNumber { get; set; }
    public List<T> Items { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}