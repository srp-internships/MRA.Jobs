using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
{
    public void Configure(EntityTypeBuilder<TestResult> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.Property(tr => tr.CompletedAt).HasColumnType("datetime2");
        builder.Property(tr => tr.Passed).HasColumnType("bit");
        builder.Property(tr => tr.Score).HasColumnType("int");

        builder.HasOne(tr => tr.Test)
            .WithMany(t => t.Results)
            .HasForeignKey(tr => tr.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tr => tr.Application)
            .WithOne(a => a.TestResult)
            .HasForeignKey<TestResult>(tr => tr.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

