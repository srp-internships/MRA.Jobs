using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Contracts.Internships.Queries;
public class GetInternshipVacancyByIdQuery : IRequest<InternshipVacancyResponce>
{
    public Guid Id { get; set; }
}
