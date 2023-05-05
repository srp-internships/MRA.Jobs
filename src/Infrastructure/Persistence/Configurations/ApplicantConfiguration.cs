using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
{
    public void Configure(EntityTypeBuilder<Applicant> builder)
    {
        builder.ToTable(nameof(User));
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(a => a.Id);
        builder.ToTable(nameof(User));
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

    }
}
