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
    public class ParentageSwineConfig : IEntityTypeConfiguration<ParentageEntity>
    {
        public void Configure(EntityTypeBuilder<ParentageEntity> entityBuilder)
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

            entityBuilder.HasOne<SwineEntity>()
                .WithOne()                              // sem navegação no principal (opcional)
                .HasForeignKey<ParentageEntity>(p => p.Id)  // FK é a própria PK de Parentage
                .OnDelete(DeleteBehavior.Restrict);     // ou Cascade, conforme sua regra
        }
    }
}
