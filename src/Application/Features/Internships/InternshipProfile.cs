using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.Internships;
public class InternshipProfile : Profile
{
    public InternshipProfile()
    {
        CreateMap<Internship, InternshipListDTO>();
        CreateMap<Internship, InternshipDetailsDTO>();
        CreateMap<CreateInternshipCommand, Internship>();
        CreateMap<UpdateInternshipCommand, Internship>();
    }
}
