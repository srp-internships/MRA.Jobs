using AutoMapper;
using MRA.Identity.Application.Contract.Messages.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Messages;
public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<SendMessageCommand, Message>();
    }
}
