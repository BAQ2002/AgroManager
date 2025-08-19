using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using MODEL;

namespace INFRA;

public class AgroManagerDbContext : DbContext
{
    public AgroManagerDbContext(DbContextOptions<AgroManagerDbContext> options) : base(options) { }

    // Exponha DbSets apenas do que precisa consultar diretamente.
    // Use os tipos do SEU projeto MODEL (ex.: Animal, Batch, AnimalBatch, MilkRecord, Pasture...)
    public DbSet<BovineEntity> Bovines => Set<BovineEntity>();
    public DbSet<SwineEntity> Swines => Set<SwineEntity>();
    public DbSet<MilkEntity> MilkRecords => Set<MilkEntity>();
    public DbSet<ParentageEntity> Parentages => Set<ParentageEntity>();
    #region Association Entities -------------------------
    public DbSet<ParentageEntity> Filiations => Set<ParentageEntity>();
    public DbSet<BatchAnimalEntity> BatchAnimals => Set<BatchAnimalEntity>();
     #endregion
    //public DbSet<PastureSegment> PastureSegments => Set<PastureSegment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica mapeamentos via Fluent API
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgroManagerDbContext).Assembly);

        modelBuilder.Entity<AnimalEntity>().UseTpcMappingStrategy(); // raiz
        // Convenções úteis (ex.: snake_case — opcional)
        // modelBuilder.HasPostgresExtension("uuid-ossp");
        // modelBuilder.UseIdentityByDefaultColumns(); // padrão no Npgsql para Identity
    }
}
