// AnimalEntityConfig.cs (resumo do essencial)
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace INFRA.Persistences 
{
    public class AnimalConfig : IEntityTypeConfiguration<AnimalEntity>
    {
        public void Configure(EntityTypeBuilder<AnimalEntity> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            #region-Propriedades Hereditarias--------------------------------

            entityBuilder.Property(x => x.Name).HasMaxLength(120); //string
            entityBuilder.Property(x => x.Tag).HasMaxLength(50); // String // novo
            entityBuilder.Property(x => x.Origin).HasConversion<int>(); //enum
            entityBuilder.Property(x => x.Gender).HasConversion<int>(); //enum
            entityBuilder.Property(x => x.BirthDate).HasColumnType("date"); //DateOnly
            entityBuilder.Property(x => x.PurchaseDate).HasColumnType("date"); //DateOnly
            entityBuilder.Property(x => x.DeathDate).HasColumnType("date"); //DateOnly
            entityBuilder.Property(x => x.PhotoKey).HasMaxLength(512);

            #endregion-------------------------------------------------------      
        }
    }
}


