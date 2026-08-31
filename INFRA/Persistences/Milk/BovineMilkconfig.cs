using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    /// <summary>
    /// Bovine-specific milk persistence mapping.
    /// </summary>
    public sealed class BovineMilkConfig : IEntityTypeConfiguration<BovineMilk>
    {
        public void Configure(EntityTypeBuilder<BovineMilk> entityBuilder)
        {
            entityBuilder.ToTable("BovineMilkRecords");

            entityBuilder.Property(x => x.BovineId)
                .HasColumnType("uuid")
                .ValueGeneratedNever();


            entityBuilder.HasIndex(x => new { x.BovineId, x.OccurrenceDate });

            entityBuilder.HasOne<BovineEntity>()
                .WithMany()
                .HasForeignKey(x => x.BovineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}