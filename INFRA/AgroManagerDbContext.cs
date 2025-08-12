using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using MODEL;
// using AgroManager.MODEL;  // <- ajuste para o namespace real do seu projeto MODEL

namespace INFRA;

public class AgroManagerDbContext : DbContext
{
    public AgroManagerDbContext(DbContextOptions<AgroManagerDbContext> options) : base(options) { }

    // Exponha DbSets apenas do que precisa consultar diretamente.
    // Use os tipos do SEU projeto MODEL (ex.: Animal, Batch, AnimalBatch, MilkRecord, Pasture...)
    public DbSet<BovineEntity> Bovines => Set<BovineEntity>();
    //public DbSet<Batch> Batches => Set<Batch>();
    //public DbSet<AnimalBatch> AnimalBatches => Set<AnimalBatch>();
    public DbSet<SwineEntity> Swines => Set<SwineEntity>();
    public DbSet<MilkEntity> MilkRecords => Set<MilkEntity>();
    public DbSet<PastureEntity> Pastures => Set<PastureEntity>();
    public DbSet<FiliationEntity> Filiations => Set<FiliationEntity>();
    public DbSet<PastureSegment> PastureSegments => Set<PastureSegment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica mapeamentos via Fluent API
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgroManagerDbContext).Assembly);

        // Convenções úteis (ex.: snake_case — opcional)
        // modelBuilder.HasPostgresExtension("uuid-ossp");
        // modelBuilder.UseIdentityByDefaultColumns(); // padrão no Npgsql para Identity
    }
}
