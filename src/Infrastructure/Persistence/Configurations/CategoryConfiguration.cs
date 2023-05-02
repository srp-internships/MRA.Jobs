using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<VacancyCategory>
{
    public void Configure(EntityTypeBuilder<VacancyCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();


    }
}
