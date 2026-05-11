using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{

    public class BovineWeightConfig : IEntityTypeConfiguration<BovineWeight>
    {
        public void Configure(EntityTypeBuilder<BovineWeight> entityBuilder)
        {
            entityBuilder.ToTable("BovineWeightRecords");

            entityBuilder.Property(x => x.BovineId)
                .HasColumnType("uuid")
                .IsRequired();

            entityBuilder.HasIndex(x => new { x.BovineId, x.OccurrenceDate });


            entityBuilder.HasOne<BovineEntity>()
                .WithMany()
                .HasForeignKey(x => x.BovineId)
                .OnDelete(DeleteBehavior.Restrict);


        }


    }

}