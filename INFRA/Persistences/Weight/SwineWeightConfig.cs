using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Persistences.Weight
{
    internal class SwineWeightConfig
    {
        public void Configure(EntityTypeBuilder<SwineWeight> entityBuilder)
        {
            entityBuilder.ToTable("SwineWeightRecords");

            entityBuilder.Property(x => x.SwineId)
                .HasColumnType("uuid")
                .IsRequired();

            entityBuilder.HasIndex(x => new { x.SwineId, x.OccurrenceDate });


            entityBuilder.HasOne<SwineEntity>()
                .WithMany()
                .HasForeignKey(x => x.SwineId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
