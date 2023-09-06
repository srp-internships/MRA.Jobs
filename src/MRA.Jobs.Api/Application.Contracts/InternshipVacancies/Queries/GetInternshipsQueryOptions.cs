using System;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;

public class GetInternshipsQueryOptions : PagedListQuery<InternshipVacancyListResponse>
{
public string SearchText { get; set; }
public string CategorySlug { get; set; }
public bool CheckDate { get; set; }

}

