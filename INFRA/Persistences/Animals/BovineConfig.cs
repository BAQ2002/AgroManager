using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class BovineConfig : IEntityTypeConfiguration<BovineEntity>
    {
        public void Configure(EntityTypeBuilder<BovineEntity> entityBuilder)
        {
            entityBuilder.ToTable("Bovines");

            entityBuilder.HasKey(x => x.Id); //Chave primaria

            entityBuilder.Property(x => x.MaritalStatus).HasConversion<int?>();
            entityBuilder.Property(x => x.CattleType).HasConversion<int?>();

            // (Opcional) índices úteis
            // entityBuilder.HasIndex(x => x.CattleType);
            // entityBuilder.HasIndex(x => x.MaritalStatus);
        }
    }
}


