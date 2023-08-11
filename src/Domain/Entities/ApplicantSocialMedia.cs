namespace MRA.Jobs.Domain.Entities;

public class ApplicantSocialMedia : BaseEntity
{
    public string ProfileUrl { get; set; }
    public SocialMediaType Type { get; set; }

    public Guid ApplicantId { get; set; }
    public Applicant Applicant { get; set; }
}