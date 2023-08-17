using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Domain.Entities.Application>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Application> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.Property(a => a.CoverLetter).HasColumnType("nvarchar(max)");
        builder.Property(a => a.AppliedAt).HasColumnType("datetime2");
        builder.Property(a => a.Status).HasColumnType("int");

        builder.HasOne(a => a.Vacancy)
            .WithMany(v => v.Applications)
            .HasForeignKey(a => a.VacancyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Applicant)
            .WithMany(ap => ap.Applications)
            .HasForeignKey(a => a.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TestResult)
            .WithOne(tr => tr.Application)
            .HasForeignKey<TestResult>(a => a.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.History)
            .WithOne(ate => ate.Application)
            .HasForeignKey(ate => ate.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}