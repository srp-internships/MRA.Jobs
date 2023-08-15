using MRA.Jobs.Application.Contracts.Common;
using Sieve.Models;
using Sieve.Services;

namespace MRA.Jobs.Application.Common.Sieve;

public interface IApplicationSieveProcessor : ISieveProcessor
{
    PagedList<TResult> ApplyAdnGetPagedList<TSource, TResult>(SieveModel model, IQueryable<TSource> source,
        Func<TSource, TResult> converter);
}