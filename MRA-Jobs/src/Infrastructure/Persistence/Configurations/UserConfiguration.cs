using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA_Jobs.Domain.Entities;

namespace MRA_Jobs.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(u => u.Id);
        builder.HasData(new List<User>
        {
            new User()
            {
                Id = 1,
                RoleId = 1,
                FirstName = "TestName",
                LastName = "TestLastName",
                Patronymic = "text",
                BirthDay = DateTime.UtcNow,
                Age = 25,
                Email = "test@gmail.com",
                Password = "test1234",
                PhoneNumber = "+9920000000"
            },
        });
    }
}