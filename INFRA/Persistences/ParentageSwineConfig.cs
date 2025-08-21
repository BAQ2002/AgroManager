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
    public class ParentageSwineConfig : IEntityTypeConfiguration<SwineParentageEntity>
    {
        public void Configure(EntityTypeBuilder<SwineParentageEntity> entityBuilder)
        {
            entityBuilder.ToTable("SwineParentages");
            entityBuilder.HasKey(x => x.Id);
           
            entityBuilder.Property(x => x.BreedingType).HasConversion<int?>();

            entityBuilder.Property(x => x.FatherId).HasColumnType("uuid");
            entityBuilder.Property(x => x.MotherId).HasColumnType("uuid");
            entityBuilder.Property(x => x.SurrogateMotherId).HasColumnType("uuid");

            entityBuilder.Property(x => x.FatherFlag).HasConversion<int?>();
            entityBuilder.Property(x => x.MotherFlag).HasConversion<int?>();
            entityBuilder.Property(x => x.SurrogateMotherFlag).HasConversion<int?>();

            // 1:1 Parentage <-> Swine (sem navegação no principal)
            entityBuilder.HasOne<SwineEntity>()
                .WithOne()                                  // sem navegação no principal (opcional)
                .HasForeignKey<ParentageEntity>(x => x.Id)  // FK é a própria PK de Parentage
                .OnDelete(DeleteBehavior.Restrict)          // ou Cascade, conforme sua regra
                .HasConstraintName("fk_parentage_swine_animal");
            
            // N:1 Pai (opcional) — sem navegação reversa
            entityBuilder.HasOne<SwineEntity>()
                .WithMany()
                .HasForeignKey(x => x.FatherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_parentage_swine_father");

            // N:1 Mãe (opcional) — sem navegação reversa
            entityBuilder.HasOne<SwineEntity>()
                .WithMany()
                .HasForeignKey(x => x.MotherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_parentage_swine_mother");

            // N:1 Mãe de aluguel (opcional)
            entityBuilder.HasOne<SwineEntity>()
                .WithMany()
                .HasForeignKey(x => x.SurrogateMotherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_parentage_swine_surrogate");
        }
    }
}
