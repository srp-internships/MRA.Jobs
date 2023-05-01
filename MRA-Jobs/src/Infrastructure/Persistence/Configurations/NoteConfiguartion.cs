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
        builder.HasData(new Note()       
        {
            User = new User
            {
                Age = 30,
                Email = "",
                Password = "",
                Patronymic = "",
                BirthDay = DateTime.UtcNow,
                FirstName = "",
                LastName = "",
                PhoneNumber = "",
                RoleId = 1
            },
            Date = DateTime.UtcNow,
            Description = "",
            AplicationId = 1
        });
    }
}