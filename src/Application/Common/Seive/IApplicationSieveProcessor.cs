using MRA.Jobs.Application.Contracts.Common;
using Sieve.Models;
using Sieve.Services;

namespace MRA.Jobs.Application.Common.Seive;

public interface IApplicationSieveProcessor : ISieveProcessor
{
    PaggedList<TResult> ApplyAdnGetPaggedList<TSource, TResult>(SieveModel model, IQueryable<TSource> source, Func<TSource, TResult> converter);
}
