﻿using System.Text.Json.Serialization;

namespace MRA.Jobs.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationStatus
{
    Submitted,
    TestCompleted,
    Verified,
    InterviewPassed,
    Approved,
    Hired,
    Reserved,
    Expired,
    Refused,
    TestFailed,
    InterviewFailed,
    Rejected
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male = 0,
    Female = 1,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LanguageCourse
{
    Russian,
    English,
    Tajik
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SocialMediaType
{
    LinkedIn,
    GitHub,
    Twitter,
    Facebook,
    Instagram,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TimelineEventType
{
    Created = 0,
    Updated = 1,
    Deleted = 2,
    StatusChanged = 3,
    Note = 4,
    Error = 5
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TrainingFormat
{
    Online,
    Offline,
    OnlineAndOffline
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkSchedule
{
    FullTime = 1,
    Flexible = 2
}
