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
    public class BovinePastureBatch : IEntityTypeConfiguration<BovineEntity>
    {
        public void Configure(EntityTypeBuilder<BovineEntity> entityBuilder)
        {
            entityBuilder.ToTable("Bovines");

            entityBuilder.Property(x => x.MaritalStatus).HasConversion<int?>(); //enum
            entityBuilder.Property(x => x.CattleType).HasConversion<int?>(); //enum
        }
    }
}
