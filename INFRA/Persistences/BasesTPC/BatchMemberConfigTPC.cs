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
    public class BatchMemberConfig : IEntityTypeConfiguration<BatchMemberEntity>
    {
        public void Configure(EntityTypeBuilder<BatchMemberEntity> entityBuilder)
        {
            #region-Propriedades Hereditarias-------------------------

            entityBuilder.Property(x => x.AnimalId).HasColumnType("uuid");
            entityBuilder.Property(x => x.BatchId).HasColumnType("uuid");
            entityBuilder.Property(x => x.BatchEntryDate).HasColumnType("timestamptz"); //DateTimeOffset
            entityBuilder.Property(x => x.BatchExitDate).HasColumnType("timestamptz"); //DateTimeOffset
            entityBuilder.Property(x => x.ExitReason).HasMaxLength(120); //string

            #endregion------------------------------------------------
        }
    }
}
