using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Jobs.Application.Contracts.Vacancy.Responces;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Contracts.Vacancy.Queries;
using MRA.Jobs.Domain.Entities;
public class GetFilteredVacanciesQuery : IRequest<IEnumerable<Vacancy>>
{
    public GetFilteredVacanciesQuery(int? requiredYearOfExperience, WorkSchedule? workSchedule, Guid? categoryId, string title)
    {
        RequiredYearOfExperience = requiredYearOfExperience;
        WorkSchedule = workSchedule;
        CategoryId = categoryId;
        Title = title;
    }

    public int? RequiredYearOfExperience { get; }
    public WorkSchedule? WorkSchedule { get; }
    public Guid? CategoryId { get; }
    public string Title { get; }
}
