using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.VacancyCategories;
public class VacancyCategoryProfile:Profile
{
    public VacancyCategoryProfile()
    {
        CreateMap<CreateVacancyCategoryCommand, VacancyCategory>();
        CreateMap<UpdateVacancyCategoryCommand,VacancyCategory>();
        CreateMap<VacancyCategory, VacancyCategoryResponce>();
        CreateMap<List<VacancyCategory>, List<VacancyCategoryResponce>>();
    }
}
