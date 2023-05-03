using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class CreateApplicationCommand:IRequest<long>
{
    public long ApplicantId { get; set; }
    public long VacancyId { get; set; }
    public string ApplicantCvPath { get; set; }
    public string ApplicantAbout { get; set; }
    public DateTime ApplicationDate { get; set; }
    public long StatusId { get; set; }
}
