using Sieve.Models;
using Sieve.Services;

public class SieveService<T> where T : class
{
    private readonly ISieveProcessor _sieveProcessor;

    public SieveService(ISieveProcessor sieveProcessor)
    {
        _sieveProcessor = sieveProcessor;
    }

    public IQueryable<T> ApplySieve(IQueryable<T> source, SieveQuery query)
    {
        var sieveModel = new SieveModel
        {
            Filters = query.Filter,
            Sorts = query.Sort,
            Page = query.Page,
            PageSize = query.PageSize
        };

        return _sieveProcessor.Apply(sieveModel, source);
    }
}
