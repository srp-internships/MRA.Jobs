using Microsoft.Extensions.Options;
using System.Reflection;
using MRA.Jobs.Application.Contracts.Common;
using Sieve.Models;
using Sieve.Services;

namespace MRA.Jobs.Infrastructure;

public class ApplicationSieveProcessor : SieveProcessor, IApplicationSieveProcessor
{
    public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options) { }
    public ApplicationSieveProcessor(IOptions<SieveOptions> options, ISieveCustomFilterMethods sieveCustomFilter) : base(options, sieveCustomFilter) { }

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
        mapper.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        return mapper;
    }
}

public class SieveCustomFilterMethods : ISieveCustomFilterMethods
{

}