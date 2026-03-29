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
            entityBuilder.HasKey(x => x.Id);

            #region-Propriedades Hereditarias-------------------------

            entityBuilder.Property(x => x.AnimalId).HasColumnType("uuid");
            entityBuilder.Property(x => x.BatchId).HasColumnType("uuid");
            entityBuilder.Property(x => x.BatchEntryDate).HasColumnType("timestamptz"); //DateTimeOffset
            entityBuilder.Property(x => x.BatchExitDate).HasColumnType("timestamptz"); //DateTimeOffset
            entityBuilder.Property(x => x.ExitReason).HasMaxLength(120); //string


            entityBuilder.HasIndex(x => x.AnimalId);
            entityBuilder.HasIndex(x => x.BatchId);
            entityBuilder.HasIndex(x => new { x.AnimalId, x.BatchExitDate });

            // Regra "apenas 1 ativo por animal" (PostgreSQL partial index)
            entityBuilder.HasIndex(x => x.AnimalId)
                .HasFilter("\"BatchExitDate\" IS NULL")
                .IsUnique();

            #endregion------------------------------------------------
        }
    }
}
