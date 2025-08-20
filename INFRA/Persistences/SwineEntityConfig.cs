using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace AgroManager.Infrastructure.Persistence.Configurations;

public class SwineEntityConfig : IEntityTypeConfiguration<SwineEntity>
{
    public void Configure(EntityTypeBuilder<SwineEntity> entityBuilder)
    {
        // Tabela única para suínos (também recebe as colunas herdadas de AnimalEntity)
        entityBuilder.ToTable("Swines");
        entityBuilder.HasKey(x => x.Id);

        // Específica de suíno (enum opcional -> int NULL):
        entityBuilder.Property(x => x.PorcType).HasConversion<int?>();

        // (Opcional) índices
        // entityBuilder.HasIndex(x => x.PorcType);
    }
}
