﻿namespace MRA.Jobs.Application.Contracts.Applicant.Commands;

public class RemoveTagsFromApplicantCommand : IRequest<bool>
{
    public Guid ApplicantId { get; set; }
    public string[] Tags { get; set; }
}