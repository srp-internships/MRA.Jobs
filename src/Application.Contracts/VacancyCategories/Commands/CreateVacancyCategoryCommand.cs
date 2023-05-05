using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
public class CreateVacancyCategoryCommand:IRequest<Guid>
{
    public string Name { get; set; }
}
