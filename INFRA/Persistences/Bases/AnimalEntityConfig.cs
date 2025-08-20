// AnimalEntityConfig.cs (resumo do essencial)
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MODEL;

namespace AgroManager.Infrastructure.Persistence.Configurations;

public class AnimalEntityConfig : IEntityTypeConfiguration<AnimalEntity>
{
    public void Configure(EntityTypeBuilder<AnimalEntity> entityBuilder)
    {
        entityBuilder.UseTpcMappingStrategy();

        #region-Propriedades Hereditarias--------------------------------
        
        entityBuilder.Property(x => x.Name).HasMaxLength(120); //string
        entityBuilder.Property(x => x.Origin).HasConversion<int>(); //enum
        entityBuilder.Property(x => x.Gender).HasConversion<int>(); //enum
        entityBuilder.Property(x => x.BirthDate).HasColumnType("date"); //DateOnly
        entityBuilder.Property(x => x.PurchaseDate).HasColumnType("date"); //DateOnly
        entityBuilder.Property(x => x.DeathDate).HasColumnType("date"); //DateOnly

        #endregion-------------------------------------------------------      
    }
}
