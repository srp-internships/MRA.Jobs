using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Application.Contracts.Applicant.Responses;
public class ApplicantSocialMediaDto
{
    public string ProfileUrl { get; set; }
    public SocialMediaType Type { get; set; }
}
