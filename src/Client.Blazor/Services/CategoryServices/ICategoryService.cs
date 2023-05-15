using MRA.Jobs.Application.Contracts.Applications.Queries;

namespace MRA.Jobs.Client.Services.CategoryServices;

public interface ICategoryService
{
    Task<List<GetApplicationsQuery>> GetAllCategory();
}
