using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
public class UpdateVacancyCategoryCommand:IRequest<long>
{
    public long Id { get; set; }
    public string Name { get; set; }
}
