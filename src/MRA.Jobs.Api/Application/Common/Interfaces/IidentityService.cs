using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

namespace MRA.Jobs.Application.Common.Interfaces;
public interface IidentityService
{
    Task<UserProfileResponse> ApplicantDetailsInfo(string userName = null);
}
