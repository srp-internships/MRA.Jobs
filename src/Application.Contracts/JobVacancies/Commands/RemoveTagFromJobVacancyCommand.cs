using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class RemoveTagFromJobVacancyCommand : IRequest<bool>
{
    public long JobVacancyId { get; set; }
    public long TagId { get; set; }
}


