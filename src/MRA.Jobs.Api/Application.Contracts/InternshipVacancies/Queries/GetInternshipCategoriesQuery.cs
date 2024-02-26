using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
public record GetInternshipCategoriesQuery : IRequest<List<InternshipCategoriesResponse>>
{
    public bool CheckDate { get; set; }
}
