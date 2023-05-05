using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyTag.Commands;
public class UpdateVacancyTagCommand: IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid VacancyId { get; set; }
    public Guid TagId { get; set; }
}
