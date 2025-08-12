using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace AgroManager.Infrastructure.Persistence.Configurations;

public class SwineEntityConfig : IEntityTypeConfiguration<SwineEntity>
{
    public void Configure(EntityTypeBuilder<SwineEntity> b)
    {
        // Tabela única para suínos (também recebe as colunas herdadas de AnimalEntity)
        b.ToTable("Swines");
        b.HasKey(x => x.Id);

        // Específica de suíno (enum opcional -> int NULL):
        b.Property(x => x.PorcType).HasConversion<int?>();

        // (Opcional) índices
        // b.HasIndex(x => x.PorcType);
    }
}
