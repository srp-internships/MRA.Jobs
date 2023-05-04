using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class VacancyTagConfiguration : IEntityTypeConfiguration<VacancyTag>
{
    public void Configure(EntityTypeBuilder<VacancyTag> builder)
    {
        builder.HasKey(vt => new { vt.VacancyId, vt.TagId });

        builder.HasOne(vt => vt.Vacancy)
            .WithMany(v => v.VacancyTags)
            .HasForeignKey(vt => vt.VacancyId);

        builder.HasOne(vt => vt.Tag)
            .WithMany(t => t.VacancyTags)
            .HasForeignKey(vt => vt.TagId);
    }
}
