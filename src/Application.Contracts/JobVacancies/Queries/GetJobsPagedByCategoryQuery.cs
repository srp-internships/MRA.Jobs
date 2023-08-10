using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;
public class GetJobsPagedByCategoryQuery: IRequest<List<JobVacancyByCategoryDTO>>
{
  public Guid CatrgoryId { get; set; }
}
