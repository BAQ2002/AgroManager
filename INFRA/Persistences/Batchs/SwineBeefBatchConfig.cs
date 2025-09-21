using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences
{
    public class SwineBeefBatchConfig : IEntityTypeConfiguration<SwineBeefBatch>
    {
        public void Configure(EntityTypeBuilder<SwineBeefBatch> entityBuilder)
        {
            entityBuilder.ToTable("SwineBeefBatchs");

        }
    }
}
