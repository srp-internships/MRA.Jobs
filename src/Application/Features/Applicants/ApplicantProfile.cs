using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Identity.Events;

namespace MRA.Jobs.Application.Features.Applicants;

using Domain.Entities;

public class ApplicantProfile : Profile
{
    public ApplicantProfile()
    {
        CreateMap<CreateApplicantCommand, Applicant>();
        CreateMap<UpdateApplicantCommand, Applicant>();
        CreateMap<NewIdentityRegisteredEvent, Applicant>();
        CreateMap<Applicant, ApplicantListDto>();
        CreateMap<Applicant, ApplicantDetailsDto>();
    }
}