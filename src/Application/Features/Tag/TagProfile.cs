using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MRA.Jobs.Application.Contracts.Tag.Commands;

namespace MRA.Jobs.Application.Features.Tag;
using MRA.Jobs.Domain.Entities;
internal class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<CreateTagCommand, Tag>();
        CreateMap<UpdateTagCommand, Tag>()
            .ForMember(e => e.Id, s => s.Ignore());
    }
}
