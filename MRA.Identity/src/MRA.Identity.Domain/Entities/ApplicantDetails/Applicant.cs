namespace MRA.Identity.Domain.Entities.ApplicantDetails;
public class Applicant
{
    public ApplicationUser User { get; set; }
    public IEnumerable<UserEducation> EducationDetails { get; set; }
    public IEnumerable<UserExperience> ExperienceDetails { get; set; }
    public IEnumerable<UserSkill> Skills { get; set; }
}
