using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<ApplicationNote>
{
    public void Configure(EntityTypeBuilder<ApplicationNote> builder)
    {
        builder.ToTable(nameof(ApplicationNote));
        builder.HasKey(n => n.Id);

        builder.HasData(new ApplicationNote()
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.UtcNow,
            Description = "",
            AplicationId = 1
        });
    }
}