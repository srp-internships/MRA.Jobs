using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;
public class EducationDetailConfiguration : IEntityTypeConfiguration<EducationDetail>
{
    public void Configure(EntityTypeBuilder<EducationDetail> builder)
    {
        builder.HasKey(ed => ed.UserId);
    }
}
