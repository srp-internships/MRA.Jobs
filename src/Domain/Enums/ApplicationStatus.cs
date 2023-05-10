namespace MRA.Jobs.Domain.Enums;
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


public enum Gender
{
    Male = 0,
    Female = 1,
}

public enum LanguageCourse
{
    Russian,
    English,
    Tajik
}

public enum SocialMediaType
{
    LinkedIn,
    GitHub,
    Twitter,
    Facebook,
    Instagram,
}

public enum TimelineEventType
{
    Created = 0,
    Updated = 1,
    Deleted = 2,
    StatusChanged = 3,
    Note = 4,
    Error = 5
}

public enum TrainingFormat
{
    Online,
    Offline,
    OnlineAndOffline
}

public enum WorkSchedule
{
    FullTime = 1,
    Flexible
}
