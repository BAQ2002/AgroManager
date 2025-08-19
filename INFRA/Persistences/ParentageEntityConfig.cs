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
    public class ParentageEntityConfig : IEntityTypeConfiguration<ParentageEntity>
    {
        public void Configure(EntityTypeBuilder<ParentageEntity> p)
        {
            // Tabela única para bovinos (contendo também as colunas herdadas de AnimalEntity)
            p.ToTable("Parentages");
            p.HasKey(x => x.Id);

            p.Property(x => x.BreedingType).HasConversion<int?>();

            p.Property(x => x.FatherId).HasColumnType("uuid");
            p.Property(x => x.MotherId).HasColumnType("uuid");
            p.Property(x => x.SurrogateMotherId).HasColumnType("uuid");                     

        }
    }
}
