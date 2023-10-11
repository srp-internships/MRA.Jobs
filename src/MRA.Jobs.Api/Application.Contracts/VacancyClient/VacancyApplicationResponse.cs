using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.Dtos.Responses;

namespace MRA.Jobs.Application.Contracts.VacancyClient;
public class VacancyApplicationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<VacancyQuestionResponseDto> VacancyQuestions { get; set; }
}
