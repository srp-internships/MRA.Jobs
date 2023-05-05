using AutoMapper;
using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Contracts.Internships.Responses;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.Internships;
public class InternshipProfile : Profile
{
    public InternshipProfile()
    {
        CreateMap<CreateInternshipCommand, Internship>();
        CreateMap<UpdateInternshipCommand, Internship>()
            .ForMember(i => i.Id, s => s.Ignore());

        CreateMap<List<Internship>, List<GetInternshipsResponse>>();
        CreateMap<Internship, GetInternshipsResponse>();
    }
}
