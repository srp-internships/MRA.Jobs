using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class TestConfiguration : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.Property(t => t.Title).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(t => t.Description).HasColumnType("nvarchar(max)");
        builder.Property(t => t.Duration).HasColumnType("time");
        builder.Property(t => t.NumberOfQuestion).HasColumnType("bigint");
        builder.Property(t => t.PassingScore).HasColumnType("int");

        builder.HasOne(t => t.Vacancy)
            .WithMany(v => v.Tests)
            .HasForeignKey(t => t.VacancyId);

        builder.HasMany(t => t.Results)
            .WithOne(tr => tr.Test)
            .HasForeignKey(tr => tr.TestId);
    }
}

