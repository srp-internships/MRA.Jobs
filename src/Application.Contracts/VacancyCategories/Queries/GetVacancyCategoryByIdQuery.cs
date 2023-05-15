using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
public class GetVacancyCategoryByIdQuery : IRequest<VacancyCategoryListDTO>
{
    public Guid Id { get; set; }
}
