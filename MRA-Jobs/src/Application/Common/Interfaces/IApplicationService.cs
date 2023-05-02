using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA_Jobs.Application.Abstractions;


namespace MRA_Jobs.Application.Common.Interfaces;
public interface IApplicationService : IEntityService<Domain.Entities.Application>
{
}
