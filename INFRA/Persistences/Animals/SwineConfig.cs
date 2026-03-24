using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class SwineConfig : IEntityTypeConfiguration<SwineEntity>
    {
        public void Configure(EntityTypeBuilder<SwineEntity> entityBuilder)
        {
            entityBuilder.ToTable("Swines");

            entityBuilder.Property(x => x.PorcType).HasConversion<int?>();
            entityBuilder.Property(x => x.Breed).HasConversion<int?>(); //enum // novo


        }
    }
}


