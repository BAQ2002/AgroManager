using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class BovinePastureMemberConfig : IEntityTypeConfiguration<BovinePastureMember>
    {
        public void Configure(EntityTypeBuilder<BovinePastureMember> entityBuilder)
        {
            entityBuilder.ToTable("BovinePastureMembers");


            entityBuilder.HasOne<BovineEntity>()
                .WithMany()
                .HasForeignKey(x => x.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne<BovinePastureBatch>()
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
