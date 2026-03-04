using Microsoft.EntityFrameworkCore;
using MODEL;

namespace INFRA;

/// <summary>
/// Repositório EF Core de suínos. Herda o comportamento comum de persistência da base genérica.
/// </summary>
public sealed class SwineRepositoryEF : AnimalRepositoryEFBase<SwineEntity>
{
    public SwineRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) : base(factory)
    {
    }

    protected override DbSet<SwineEntity> GetDbSet(AgroManagerDbContext db) => db.Swines;
}
