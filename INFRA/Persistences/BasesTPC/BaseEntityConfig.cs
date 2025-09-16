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
        public void Configure(EntityTypeBuilder<BaseEntity> entityBuilder)
        {
            entityBuilder.UseTpcMappingStrategy();

            entityBuilder.HasKey(x => x.Id); //Chave primaria

            #region-Propriedades Hereditarias------------------------------------------

            entityBuilder.Property(x => x.Id).HasColumnType("uuid").ValueGeneratedNever(); //Guid
            entityBuilder.Property(x => x.CreatedAt).HasColumnType("timestamptz"); //DateTimeOffset
            entityBuilder.Property(x => x.UpdatedAt).HasColumnType("timestamptz"); //DateTimeOffset

            #endregion-----------------------------------------------------------------
        }
    }
}
