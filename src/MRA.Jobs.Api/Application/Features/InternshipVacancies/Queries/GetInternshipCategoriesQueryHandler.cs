using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries;
public class GetInternshipCategoriesQueryHandler : IRequestHandler<GetInternshipCategoriesQuery, List<InternshipCategoriesResponce>>
{

    public Task<List<InternshipCategoriesResponce>> Handle(GetInternshipCategoriesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
