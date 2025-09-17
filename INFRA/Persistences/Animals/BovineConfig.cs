using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class BovineConfig : IEntityTypeConfiguration<BovineEntity>
    {
        public void Configure(EntityTypeBuilder<BovineEntity> b)
        {
            b.ToTable("Bovines");
            b.Property(x => x.MaritalStatus).HasConversion<int?>();
            b.Property(x => x.CattleType).HasConversion<int?>();
        }

    }
}


