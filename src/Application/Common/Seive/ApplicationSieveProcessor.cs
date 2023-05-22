using Microsoft.Extensions.Options;
using MRA.Jobs.Application.Contracts.Common;
using Sieve.Models;
using Sieve.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MRA.Jobs.Application.Common.Seive;

public class ApplicationSieveProcessor : SieveProcessor, IApplicationSieveProcessor
{
    private readonly IServiceProvider _services;
    public ApplicationSieveProcessor(IOptions<SieveOptions> options, ISieveCustomFilterMethods sieveCustomFilter, IServiceProvider services) : base(options, sieveCustomFilter)
    {
        _services = services;
    }

    public PaggedList<TResult> ApplyAdnGetPaggedList<TSource, TResult>(SieveModel model, IQueryable<TSource> source, Func<TSource, TResult> converter)
    {
        source = Apply(model, source, applyPagination: false);
        var totalCount = source.Count();
        var pageSize = model.PageSize ?? Options.Value.DefaultPageSize;
        pageSize = Math.Min(Options?.Value.MaxPageSize ?? int.MaxValue, pageSize);
        var currentPage = model.Page ?? 1;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var result = ApplyPagination(model, source).ToArray().Select(converter).ToList();
        return new PaggedList<TResult> { TotalCount = totalCount, PageSize = pageSize, TotalPages = totalPages, CurrentPageNumber = model.Page ?? 1, Items = result, HasPreviousPage = currentPage > 1, HasNextPage = currentPage < totalPages };
    }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        //var markers = _services.GetServices<ISieveConfigurationsAssemblyMarker>();
        //foreach (var marker in markers)
        mapper.ApplyConfigurationsFromAssembly(Assembly.GetEntryAssembly());

        return mapper;
    }
}