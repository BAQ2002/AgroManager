using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Persistences.Bases
{
    public class BatchEntityConfig : IEntityTypeConfiguration<BatchEntity>
    {
        public void Configure(EntityTypeBuilder<BatchEntity> entityBuilder)
        {
            entityBuilder.UseTpcMappingStrategy();

            #region-Propriedades Hereditarias-------------------------

            entityBuilder.Property(x => x.Name).HasMaxLength(120); //string
            entityBuilder.Property(x => x.Description).HasMaxLength(120); //string

            #endregion------------------------------------------------
        }
    }
}
