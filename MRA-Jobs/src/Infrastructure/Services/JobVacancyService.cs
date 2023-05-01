using MRA_Jobs.Application.Common.Models;

namespace MRA_Jobs.Infrastructure.Services;
public class JobVacancyService : IJobVacancyService
{
    public Task<ServiceResponse<TGetEntity>> Add<TAddEntity, TGetEntity>(TAddEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<bool>> Delete(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<List<TGetEntity>>> GetAll<TGetEntity>()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<TGetEntity>> GetById<TGetEntity>(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<TGetEntity>> Update<TUpdateEntity, TGetEntity>(TUpdateEntity entity)
    {
        throw new NotImplementedException();
    }
}
