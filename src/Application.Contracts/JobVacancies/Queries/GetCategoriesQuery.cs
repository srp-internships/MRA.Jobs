using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;
public class GetCategoriesQuery : IRequest<List<CategoryResponseCount>>
{
}
