using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies;
public class InternshipProfile : Profile
{
    public InternshipProfile()
    {
        CreateMap<InternshipVacancy, InternshipListDTO>();
        CreateMap<InternshipVacancy, InternshipDetailsDTO>();
        CreateMap<CreateInternshipCommand, InternshipVacancy>();
        CreateMap<UpdateInternshipCommand, InternshipVacancy>();
    }
}
