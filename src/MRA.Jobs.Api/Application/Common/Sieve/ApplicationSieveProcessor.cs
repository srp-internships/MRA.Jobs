using System.Reflection;
using Microsoft.Extensions.Options;
using MRA.Jobs.Application.Contracts.Common;
using Sieve.Models;
using Sieve.Services;

namespace MRA.Jobs.Application.Common.Sieve;

public class ApplicationSieveProcessor : SieveProcessor, IApplicationSieveProcessor
{
    private readonly IServiceProvider _services;

    public ApplicationSieveProcessor(IOptions<SieveOptions> options, ISieveCustomFilterMethods sieveCustomFilter,
        IServiceProvider services) : base(options, sieveCustomFilter)
    {
        _services = services;
    }

    public PagedList<TResult> ApplyAdnGetPagedList<TSource, TResult>(SieveModel model, IQueryable<TSource> source,
        Func<TSource, TResult> converter)
    {
        source = Apply(model, source, applyPagination: false);
        int totalCount = source.Count();
        int pageSize = model.PageSize ?? Options.Value.DefaultPageSize;
        pageSize = 10/*Math.Min(Options?.Value.MaxPageSize ?? int.MaxValue, pageSize)*/;
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

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        //var markers = _services.GetServices<ISieveConfigurationsAssemblyMarker>();
        //foreach (var marker in markers)
        mapper.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        return mapper;
    }
}