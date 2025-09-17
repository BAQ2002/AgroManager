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
    public class BovineParentageConfig : IEntityTypeConfiguration<BovineParentageEntity>
    {
        public void Configure(EntityTypeBuilder<BovineParentageEntity> b)
        {
            b.ToTable("BovineParentages");
            b.HasKey(x => x.Id);

            // Garante que as colunas existam na tabela concreta (TPC):
            b.Property(x => x.FatherId).HasColumnName("FatherId").HasColumnType("uuid");
            b.Property(x => x.MotherId).HasColumnName("MotherId").HasColumnType("uuid");
            b.Property(x => x.SurrogateMotherId).HasColumnName("SurrogateMotherId").HasColumnType("uuid");

            // 1:1 (PK compartilhada) Parentage <-> Bovine "dono" do registro
            b.HasOne<BovineEntity>()
             .WithOne()
             .HasForeignKey<BovineParentageEntity>(x => x.Id)
             .OnDelete(DeleteBehavior.Restrict)
             .HasConstraintName("fk_parentage_bovine_animal");

            // N:1 Pai/Mãe/Mãe de aluguel (todas opcionais)
            b.HasOne<BovineEntity>()
             .WithMany()
             .HasForeignKey(x => x.FatherId)
             .OnDelete(DeleteBehavior.Restrict)
             .HasConstraintName("fk_parentage_bovine_father");

            b.HasOne<BovineEntity>()
             .WithMany()
             .HasForeignKey(x => x.MotherId)
             .OnDelete(DeleteBehavior.Restrict)
             .HasConstraintName("fk_parentage_bovine_mother");

            b.HasOne<BovineEntity>()
             .WithMany()
             .HasForeignKey(x => x.SurrogateMotherId)
             .OnDelete(DeleteBehavior.Restrict)
             .HasConstraintName("fk_parentage_bovine_surrogate");
        }
    }
}
