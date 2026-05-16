using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Persistences
{
    public abstract class WeightConfig : IEntityTypeConfiguration<WeightEntity>
    {
        public void Configure(EntityTypeBuilder<WeightEntity> builder)
        {
            #region-Propriedades Hereditarias-------------------------
            
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("uuid").ValueGeneratedNever();
            builder.Property(x => x.OccurrenceDate).HasColumnType("date").IsRequired();
            builder.Property(x => x.Weight).HasColumnType("real").IsRequired();
            
            #endregion------------------------------------------------

        }

    }
}
