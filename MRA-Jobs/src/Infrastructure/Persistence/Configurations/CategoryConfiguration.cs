using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA_Jobs.Domain.Entities;

namespace MRA_Jobs.Infrastructure.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.HasMany(s => s.JobVacancies).WithOne(s => s.Category);
        builder.HasMany(s => s.EducationVacancies).WithOne(s => s.Category);

    }
}
