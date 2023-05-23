using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies;
public class InternshipProfile : Profile
{
    public InternshipProfile()
    {
        CreateMap<InternshipVacancy, InternshipVacancyListResponce>();
        CreateMap<InternshipVacancy, InternshipVacancyResponce>();
        CreateMap<CreateInternshipVacancyCommand, InternshipVacancy>();
        CreateMap<UpdateInternshipVacancyCommand, InternshipVacancy>();
    }
}
