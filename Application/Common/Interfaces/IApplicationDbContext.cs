﻿using MRA.Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<JobVacancy> JobVacancies { get; }
    DbSet<VacancyCategory> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
