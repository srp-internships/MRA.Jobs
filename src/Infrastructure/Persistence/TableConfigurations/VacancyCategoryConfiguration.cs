using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class VacancyCategoryConfiguration : IEntityTypeConfiguration<VacancyCategory>
{
    public void Configure(EntityTypeBuilder<VacancyCategory> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.Property(vc => vc.Name).HasColumnType("nvarchar(128)").IsRequired();

        builder.HasMany(vc => vc.Vacancies)
            .WithOne(v => v.Category)
            .HasForeignKey(v => v.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}