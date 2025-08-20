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
        public void Configure(EntityTypeBuilder<ParentageEntity> entityBuilder)
        {
            entityBuilder.ToTable("Parentages");
            entityBuilder.HasKey(x => x.Id);
           
            entityBuilder.Property(x => x.BreedingType).HasConversion<int?>();

            entityBuilder.Property(x => x.FatherId).HasColumnType("uuid");
            entityBuilder.Property(x => x.MotherId).HasColumnType("uuid");
            entityBuilder.Property(x => x.SurrogateMotherId).HasColumnType("uuid");

            entityBuilder.Property(x => x.FatherFlag).HasConversion<int?>();
            entityBuilder.Property(x => x.MotherFlag).HasConversion<int?>();
            entityBuilder.Property(x => x.SurrogateMotherFlag).HasConversion<int?>();

            entityBuilder.HasOne<AnimalEntity>()
               .WithMany() //Define que 1 Bovine pode estar associado a varios Milk
               .HasForeignKey(m => m.Id)
               .OnDelete(DeleteBehavior.Restrict); //impede que o principal seja deletado se existir dependente apontando para ele.
        }
    }
}
