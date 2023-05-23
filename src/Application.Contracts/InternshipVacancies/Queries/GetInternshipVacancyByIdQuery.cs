using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
public class GetInternshipVacancyByIdQuery : IRequest<InternshipVacancyResponce>
{
    public Guid Id { get; set; }
}
