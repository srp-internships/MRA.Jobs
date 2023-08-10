﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {

        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.Property(t => t.Name).HasColumnType("nvarchar(128)").IsRequired();

        builder.HasMany(t => t.VacancyTags)
            .WithOne(vt => vt.Tag)
            .HasForeignKey(vt => vt.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.UserTags)
            .WithOne(ut => ut.Tag)
            .HasForeignKey(ut => ut.TagId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

