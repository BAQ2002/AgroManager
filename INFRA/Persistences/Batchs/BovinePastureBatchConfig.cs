using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class BovinePastureBatchConfig : IEntityTypeConfiguration<BovinePastureBatch>
    {
        public void Configure(EntityTypeBuilder<BovinePastureBatch> entityBuilder)
        {
            entityBuilder.ToTable("BovinePastureBatchs");

        }
    }
}
