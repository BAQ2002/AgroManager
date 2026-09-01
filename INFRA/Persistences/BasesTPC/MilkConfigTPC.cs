using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    /// <summary>
    /// Common persistence configuration for every species-specific milk entity.
    /// </summary>
    public abstract class MilkConfigTPC : IEntityTypeConfiguration<MilkEntity>
    {
        public void Configure(EntityTypeBuilder<MilkEntity> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).HasColumnType("uuid").ValueGeneratedNever(); //Guid
            entityBuilder.Property(x => x.OccurredAt).HasColumnType("timestamp with time zone").IsRequired();
            entityBuilder.Property(x => x.Liters).HasColumnType("real");

        }
    }
}
