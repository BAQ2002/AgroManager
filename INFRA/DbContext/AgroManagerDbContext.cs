using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using MODEL;
using INFRA.Persistences;

namespace INFRA;

public class AgroManagerDbContext : DbContext
{
    public AgroManagerDbContext(DbContextOptions<AgroManagerDbContext> options) : base(options) { }

    // Exponha DbSets apenas do que precisa consultar diretamente.
    // Use os tipos do SEU projeto MODEL (ex.: Animal, Batch, AnimalBatch, MilkRecord, Pasture...)
    public DbSet<BovineEntity> Bovines => Set<BovineEntity>();
    public DbSet<SwineEntity> Swines => Set<SwineEntity>();
    public DbSet<MilkEntity> MilkRecords => Set<MilkEntity>();
    public DbSet<PastureEntity> Pastures => Set<PastureEntity>();

    #region Batch Entities -------------------------------------------------------- 

    public DbSet<BovinePastureBatch> BovinePastureBatchs => Set<BovinePastureBatch>();
    public DbSet<SwineBeefBatch> SwineBeefBatchs => Set<SwineBeefBatch>();

    #endregion--------------------------------------------------------------------------

    #region Association Entities --------------------------------------------------------

    public DbSet<BovineParentageEntity> BovineParentages => Set<BovineParentageEntity>();
    public DbSet<SwineParentageEntity> SwineParentages => Set<SwineParentageEntity>();

    public DbSet<BovinePastureMember> BovinePastureMembers => Set<BovinePastureMember>();
    public DbSet<SwineBeefMember> SwineBeefMembers => Set<SwineBeefMember>();
   
    #endregion--------------------------------------------------------------------------
    //public DbSet<PastureSegment> PastureSegments => Set<PastureSegment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica mapeamentos via Fluent API
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgroManagerDbContext).Assembly);

        // modelBuilder.Entity<BaseEntity>().UseTpcMappingStrategy();
        modelBuilder.Entity<AnimalEntity>().UseTpcMappingStrategy();
        modelBuilder.Entity<ParentageEntity>().UseTpcMappingStrategy();

        modelBuilder.ApplyConfiguration(new BovineConfig());
        modelBuilder.ApplyConfiguration(new BovineParentageConfig());

        // raiz
        // Convenções úteis (ex.: snake_case — opcional)
        // modelBuilder.HasPostgresExtension("uuid-ossp");
        // modelBuilder.UseIdentityByDefaultColumns(); // padrão no Npgsql para Identity
    }
}
