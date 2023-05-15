using MediatR;
using Sieve.Models;

namespace MRA.Jobs.Application.Contracts.Common;

public class PaggedListQuery<T> : SieveModel, IRequest<PaggedList<T>>
{
}

public class PaggedList<T>
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPageNumber { get; set; }
    public List<T> Items { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}