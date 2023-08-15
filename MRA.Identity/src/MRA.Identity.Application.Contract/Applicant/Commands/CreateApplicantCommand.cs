using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Domain.Entities.ApplicantDetails;

namespace MRA.Identity.Application.Contract.Applicant.Commands;
public class CreateApplicantCommand : IRequest<Guid?>
{
    public Guid Id { get; set; }
    public IEnumerable<UserEducation> EducationDetails { get; set; }
    public IEnumerable<UserExperience> ExperienceDetails { get; set; }
    public IEnumerable<UserSkill> Skills { get; set; }
}
