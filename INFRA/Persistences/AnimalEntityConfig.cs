// AnimalEntityConfig.cs (resumo do essencial)
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace AgroManager.Infrastructure.Persistence.Configurations;

public class AnimalEntityConfig : IEntityTypeConfiguration<AnimalEntity>
{
    public void Configure(EntityTypeBuilder<AnimalEntity> b)
    {
        b.UseTpcMappingStrategy(); // <- garante TPC (sem tabela para a classe base)
        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnType("uuid"); // você já cria Guid no ctor -> DB não precisa gerar
                                        // .ValueGeneratedNever();     // opcional (Guid já não é gerado por padrão)
        b.Property(x => x.CreatedAt).HasColumnType("timestamptz"); // timestamp com fuso (UTC-friendly)
        b.Property(x => x.UpdatedAt).HasColumnType("timestamptz");

        // Campos COMUNS da hierarquia (se quiser padronizar aqui):
        b.Property(x => x.Name).HasMaxLength(120);
        b.Property(x => x.Origin).HasConversion<int>();   // enum
        b.Property(x => x.Gender).HasConversion<int>();   // enum
        b.Property(x => x.BirthDate).HasColumnType("date");
        b.Property(x => x.PurchaseDate).HasColumnType("date");
        b.Property(x => x.DeathDate).HasColumnType("date");

        // Se quiser padronizar timestamps herdados de BaseEntity (opcional):
        // b.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        // b.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
    }
}
