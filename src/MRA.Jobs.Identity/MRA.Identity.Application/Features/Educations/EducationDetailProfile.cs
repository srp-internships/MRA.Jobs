using AutoMapper;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Educations;

public class EducationDetailProfile : Profile
{
    public EducationDetailProfile()
    {
        CreateMap<UpdateEducationDetailCommand, EducationDetail>();
        CreateMap<EducationDetail, UserEducationResponse>();
    }
}