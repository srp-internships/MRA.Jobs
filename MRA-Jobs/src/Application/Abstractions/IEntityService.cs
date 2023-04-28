using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MRA_Jobs.Application.Abstractions;
public interface IEntityService<TEntity>
         where TEntity : class
{
    Task<ServiceResponse<List<TGetEntity>>> GetAll<TGetEntity>();
    Task<ServiceResponse<TGetEntity>> GetById<TGetEntity>(long id);
    Task<ServiceResponse<bool>> Delete(long id);
    Task<ServiceResponse<TUpdateEntity>> Update<TUpdateEntity, TGetEntity>(TUpdateEntity entity);
    Task<ServiceResponse<TGetEntity>> Add<TAddEntity,TGetEntity>(TAddEntity entity);
}
