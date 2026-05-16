using Microsoft.EntityFrameworkCore;
using MODEL;

namespace INFRA.Repositories.Weight;

public sealed class SwineWeightRepositoryEF : AnimalWeightRepositoryEFBase<SwineWeight> , ISwineWeightRepository
{
    public SwineWeightRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) : base(factory) { }

    protected override DbSet<SwineWeight> GetDbSet(AgroManagerDbContext db) => db.SwineWeightRecords;

    protected override IQueryable<SwineWeight> GetByAnimalId(IQueryable<SwineWeight> query, Guid animalId)
        => query.Where(x => x.SwineId == animalId);
}