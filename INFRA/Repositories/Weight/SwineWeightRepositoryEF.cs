using Microsoft.EntityFrameworkCore;
using MODEL;

namespace INFRA;

public sealed class SwineWeightRepositoryEF : WeightRepositoryEFBase<SwineWeight> , ISwineWeightRepository
{
    public SwineWeightRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) : base(factory) { }

    protected override DbSet<SwineWeight> GetSet(AgroManagerDbContext db) => db.SwineWeightRecords;

    protected override IQueryable<SwineWeight> FilterByAnimalId(IQueryable<SwineWeight> query, Guid animalId)
        => query.Where(x => x.SwineId == animalId);
}