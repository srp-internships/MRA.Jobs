using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA_Jobs.Domain.Entities;

namespace MRA_Jobs.Infrastructure.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable(nameof(Note));
        builder.HasKey(n => n.Id);

        builder.HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId);

        builder.HasData(new Note()
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.UtcNow,
            Description = "",
            AplicationId = 1
        });
    }
}