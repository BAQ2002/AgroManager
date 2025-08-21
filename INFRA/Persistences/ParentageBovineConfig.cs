using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Persistences
{
    public class ParentageBovineConfig : IEntityTypeConfiguration<BovineParentageEntity>
    {
        public void Configure(EntityTypeBuilder<BovineParentageEntity> entityBuilder)
        {
            entityBuilder.ToTable("BovineParentages");
            
            // 1:1 Parentage <-> Bovine (sem navegação no principal)
            entityBuilder.HasOne<BovineEntity>()
                .WithOne()                                  // sem navegação no principal (opcional)
                .HasForeignKey<ParentageEntity>(x => x.Id)  // FK é a própria PK de Parentage
                .OnDelete(DeleteBehavior.Restrict)          // ou Cascade, conforme sua regra
                .HasConstraintName("fk_parentage_bovine_animal");

            // N:1 Pai (opcional) — sem navegação reversa
            entityBuilder.HasOne<BovineEntity>()
             .WithMany()
             .HasForeignKey(x => x.FatherId)
             .OnDelete(DeleteBehavior.Restrict)
             .HasConstraintName("fk_parentage_bovine_father");

            // N:1 Mãe (opcional) — sem navegação reversa
            entityBuilder.HasOne<BovineEntity>()
             .WithMany()
             .HasForeignKey(x => x.MotherId)
             .OnDelete(DeleteBehavior.Restrict)
             .HasConstraintName("fk_parentage_bovine_mother");

            // N:1 Mãe de aluguel (opcional)
            entityBuilder.HasOne<BovineEntity>()
             .WithMany()
             .HasForeignKey(x => x.SurrogateMotherId)
             .OnDelete(DeleteBehavior.Restrict)
             .HasConstraintName("fk_parentage_bovine_surrogate");

            // Índices
            //b.HasIndex(x => x.AnimalId).IsUnique().HasDatabaseName("ux_parentage_bovine_animal");
            //b.HasIndex(x => x.SireId).HasDatabaseName("ix_parentage_bovine_sire");
            //b.HasIndex(x => x.DamId).HasDatabaseName("ix_parentage_bovine_dam");
            //b.HasIndex(x => x.SurrogateMotherId).HasDatabaseName("ix_parentage_bovine_surrogate");
            //b.HasIndex(x => new { x.SireId, x.DamId }).HasDatabaseName("ix_parentage_bovine_sire_dam");

            // Checks de sanidade (opcional, mas recomendados)
            //b.HasCheckConstraint("ck_parentage_bovine_no_self_sire", "SireId IS NULL OR SireId <> AnimalId");
            //b.HasCheckConstraint("ck_parentage_bovine_no_self_dam", "DamId  IS NULL OR DamId  <> AnimalId");
            //b.HasCheckConstraint("ck_parentage_bovine_no_self_surrogate", "SurrogateMotherId IS NULL OR SurrogateMotherId <> AnimalId");
            //b.HasCheckConstraint("ck_parentage_bovine_sire_ne_dam",
            //    "SireId IS NULL OR DamId IS NULL OR SireId <> DamId");
        }
    }
}
