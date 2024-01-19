using AutoMapper;
using MRA.Identity.Application.Contract.Messages.Commands;
using MRA.Identity.Application.Contract.Messages.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Messages;
public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<SendMessageCommand, Message>();
        CreateMap<Message, GetMessageResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
    }
}
