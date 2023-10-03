using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
public record GetInternshipCategoriesQuery : IRequest<List<InternshipCategoriesResponce>>
{
    public bool CheckDate { get; set; }
}
