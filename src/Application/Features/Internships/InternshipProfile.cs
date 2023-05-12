using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.Internships;
public class InternshipProfile : Profile
{
    public InternshipProfile()
    {
        CreateMap<CreateInternshipCommand, Internship>();
        CreateMap<UpdateInternshipCommand, Internship>();
        CreateMap<Internship, InternshipDetailsDTO>();
        CreateMap<Internship, InternshipListDTO>();
    }
}
