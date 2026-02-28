using INFRA.Persistences;
using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace INFRA;
/// <summary>
/// Unidade de trabalho EF Core
/// <para>1.Conexão e execução de Querie's : Ela usa a configuração recebida(DbContextOptions) para acessar o banco(provider, connection string etc.).</para>
/// <para>2.Mapeamento objeto ⭠⭢ tabela : Expõe DbSets para entidades(Bovines, Swines...) e sabe como mapear tudo via OnModelCreating.</para>
/// <para>3.Change Tracking : Quando você adiciona/altera/remove entidades, o contexto rastreia estado(Added/Modified/Deleted).</para>
/// <para>4.Persistência(SaveChanges) : Só quando chama SaveChangesAsync ele transforma mudanças em SQL e envia ao banco.</para>
/// </summary>
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

        modelBuilder.Entity<AnimalEntity>().UseTpcMappingStrategy();
        modelBuilder.Entity<ParentageEntity>().UseTpcMappingStrategy();
        modelBuilder.Entity<BatchEntity>().UseTpcMappingStrategy();
        modelBuilder.Entity<BatchMemberEntity>().UseTpcMappingStrategy();

        modelBuilder.ApplyConfiguration(new BovineConfig());
        modelBuilder.ApplyConfiguration(new BovineParentageConfig());

        // raiz
        // Convenções úteis (ex.: snake_case — opcional)
        // modelBuilder.HasPostgresExtension("uuid-ossp");
        // modelBuilder.UseIdentityByDefaultColumns(); // padrão no Npgsql para Identity
    }
}
