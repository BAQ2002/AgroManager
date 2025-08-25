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
    public class MilkConfig : IEntityTypeConfiguration<MilkEntity>
    {
        public void Configure(EntityTypeBuilder<MilkEntity> entityBuilder)
        {
            entityBuilder.ToTable("MilkRecords");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.BovineId).HasColumnType("uuid").ValueGeneratedNever();
            entityBuilder.Property(x => x.OccurrenceDate).HasColumnType("date");
            entityBuilder.Property(x => x.Liters).HasColumnType("real");

            // Índice na FK melhora performance em JOINs
            entityBuilder.HasIndex(x => x.BovineId);

            entityBuilder.HasOne<BovineEntity>()
               .WithMany() //Define que 1 Bovine pode estar associado a varios Milk
               .HasForeignKey(m => m.BovineId)
               .OnDelete(DeleteBehavior.Restrict); //impede que o principal seja deletado se existir dependente apontando para ele.
        }
    }
}
