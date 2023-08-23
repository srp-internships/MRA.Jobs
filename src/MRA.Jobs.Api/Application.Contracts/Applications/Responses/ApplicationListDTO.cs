﻿using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Domain.Entities;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.Applications.Responses;

public class ApplicationListDto
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public string CoverLetter { get; set; }
    public Guid VacancyId { get; set; }
    public int StatusId { get; set; }
    public IEnumerable<JobQuestion> JobQuestions { get; set; }
}

public class ApplicationDetailsDto
{
    public Guid Id { get; set; }

    public Guid ApplicantId { get; set; }
    public string CoverLetter { get; set; }
    public Guid VacancyId { get; set; }
    public int StatusId { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? LastModifiedAt { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public IQueryable<TimeLineDetailsDto> Histiry { get; set; }
}

public class ApplicationListStatus
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid VacancyId { get; set; }
    public string VacancyTitle { get; set; }
    public ApplicationStatus Status { get; set; }
}