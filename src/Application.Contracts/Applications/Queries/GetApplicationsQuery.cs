using System;
using MediatR;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;
public class GetApplicationsQuery : IRequest<List<ApplicationResponse>>
{
}
