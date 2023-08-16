using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;
public class ExperienceDetailConfiguration : IEntityTypeConfiguration<ExperienceDetail>
{
    public void Configure(EntityTypeBuilder<ExperienceDetail> builder)
    {
        builder.HasKey(ed => ed.UserId);
    }
}
