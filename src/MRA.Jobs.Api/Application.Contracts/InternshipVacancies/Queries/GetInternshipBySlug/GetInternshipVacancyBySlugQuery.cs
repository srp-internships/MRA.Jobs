﻿using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
public class GetInternshipVacancyBySlugQuery : IRequest<InternshipVacancyResponse>
{
    public string Slug { get; set; }
}
