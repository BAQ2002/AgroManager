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
    public class BaseEntityConfig : IEntityTypeConfiguration<BaseEntity>
    {
        public void Configure(EntityTypeBuilder<BaseEntity> b)
        {
            b.UseTpcMappingStrategy();
            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnType("uuid");
            b.Property(x => x.CreatedAt).HasColumnType("timestamptz"); 
            b.Property(x => x.UpdatedAt).HasColumnType("timestamptz");

        }
    }
}
