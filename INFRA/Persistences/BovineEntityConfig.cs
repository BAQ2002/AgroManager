using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace AgroManager.Infrastructure.Persistence.Configurations;

public class BovineEntityConfig : IEntityTypeConfiguration<BovineEntity>
{
    public void Configure(EntityTypeBuilder<BovineEntity> b)
    {
        // Tabela única para bovinos (contendo também as colunas herdadas de AnimalEntity)
        b.ToTable("Bovines");
        b.HasKey(x => x.Id);

        // Específicas de bovino (enums opcionais -> int NULL):
        b.Property(x => x.MaritalStatus).HasConversion<int?>();
        b.Property(x => x.CattleType).HasConversion<int?>();

        // (Opcional) índices úteis
        // b.HasIndex(x => x.CattleType);
        // b.HasIndex(x => x.MaritalStatus);
    }
}
