namespace MRA.Jobs.Domain.Entities;

public class Applicant : User
{
    public ICollection<Application> Applications { get; set; }
    public ICollection<ApplicantSocialMedia> SocialMedias { get; set; }
}