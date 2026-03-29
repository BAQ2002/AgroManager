using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class BovinePastureBatchConfig : IEntityTypeConfiguration<BovinePastureBatch>
    {
        public void Configure(EntityTypeBuilder<BovinePastureBatch> entityBuilder)
        {
            entityBuilder.ToTable("BovinePastureBatchs");

            entityBuilder.Property(x => x.PastureId)
                .HasColumnType("uuid")
                .IsRequired();

            entityBuilder.HasOne<PastureEntity>()
                .WithMany()
                .HasForeignKey(x => x.PastureId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
