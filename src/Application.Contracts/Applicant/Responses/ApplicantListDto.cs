﻿using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

using MRA.Jobs.Application.Contracts.Converter.Converter;
using Newtonsoft.Json;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.Applicant.Responses;

public class ApplicantListDto
{
    public Guid Id { get; set; }
    public string Avatar { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}

public class ApplicantDetailsDto
{
    public Guid Id { get; set; }
    public string Avatar { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public ICollection<TimeLineDetailsDto> History { get; set; }
    public ICollection<TagDto> Tags { get; set; }
    public ICollection<ApplicantSocialMediaDto> SocialMedias { get; set;}
}
