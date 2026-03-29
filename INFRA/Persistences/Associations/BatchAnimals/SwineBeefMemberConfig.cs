using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class SwineBeefMemberConfig : IEntityTypeConfiguration<SwineBeefMember>
    {
        public void Configure(EntityTypeBuilder<SwineBeefMember> entityBuilder)
        {
            entityBuilder.ToTable("SwineBeefMembers");

            entityBuilder.HasOne<SwineEntity>()
                .WithMany()
                .HasForeignKey(x => x.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne<SwineBeefBatch>()
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
