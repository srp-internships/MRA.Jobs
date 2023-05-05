using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Application.Contracts.Applications.Responses;
public class ApplicationResponse
{
    public long ApplicantId { get; set; }
    public string CoverLetter { get; set; }
    public string History { get; set; }
    public long VacancyId { get; set; }
    public string ResumeUrl { get; set; }
    public int StatusId { get; set; }
}
