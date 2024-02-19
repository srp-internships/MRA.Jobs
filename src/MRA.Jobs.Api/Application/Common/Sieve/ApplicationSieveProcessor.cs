using System.Reflection;
using Microsoft.Extensions.Options;
using MRA.Jobs.Application.Contracts.Common;
using Sieve.Models;
using Sieve.Services;

namespace MRA.Jobs.Application.Common.Sieve;

public class ApplicationSieveProcessor(
    IOptions<SieveOptions> options,
    ISieveCustomFilterMethods sieveCustomFilter)
    : SieveProcessor(options, sieveCustomFilter), IApplicationSieveProcessor
{
    public PagedList<TResult> ApplyAdnGetPagedList<TSource, TResult>(SieveModel model, IQueryable<TSource> source,
        Func<TSource, TResult> converter)
    {
        try
        {
        source = Apply(model, source, applyPagination: false);
        int totalCount = source.Count();
        int pageSize = model.PageSize ?? Options.Value.DefaultPageSize;
        pageSize = Math.Min(Options?.Value.MaxPageSize ?? int.MaxValue, pageSize);
        int currentPage = model.Page ?? 1;
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        List<TResult> result = ApplyPagination(model, source).ToArray().Select(converter).ToList();
        return new PagedList<TResult>
        {
            TotalCount = totalCount,
            PageSize = pageSize,
            TotalPages = totalPages,
            CurrentPageNumber = model.Page ?? 1,
            Items = result,
            HasPreviousPage = currentPage > 1,
            HasNextPage = currentPage < totalPages
        };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        return mapper;
    }
}