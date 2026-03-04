using Microsoft.EntityFrameworkCore;
using MODEL;

namespace INFRA;

/// <summary>
/// Repositório EF Core de bovinos. Herda o comportamento comum de persistência da base genérica.
/// </summary>
public sealed class BovineRepositoryEF : AnimalRepositoryEFBase<BovineEntity>
{
    public BovineRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) : base(factory)
    {
    }

    protected override DbSet<BovineEntity> GetDbSet(AgroManagerDbContext db) => db.Bovines;
}
