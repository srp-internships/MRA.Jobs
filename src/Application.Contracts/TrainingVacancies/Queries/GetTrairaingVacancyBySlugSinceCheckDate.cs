using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public class GetTrairaingVacancyBySlugSinceCheckDate: IRequest<TrainingVacancyDetailedResponse>
{
    public string Slug { get; set; }
}
