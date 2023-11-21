using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface ICvService
{
    Task<string> GetCvByCommandAsync(ref CreateApplicationCommand command);
}