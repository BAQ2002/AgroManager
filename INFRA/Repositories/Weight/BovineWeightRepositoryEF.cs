using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Repositories.Weight
{
    using Microsoft.EntityFrameworkCore;
    using MODEL;

    public sealed class BovineWeightRepositoryEF : WeightRepositoryEFBase<BovineWeight> , IBovineWeightRepository
    {
        public BovineWeightRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) : base(factory) { }

        protected override DbSet<BovineWeight> GetSet(AgroManagerDbContext db) => db.BovineWeightRecords;

        protected override IQueryable<BovineWeight> FilterByAnimalId(IQueryable<BovineWeight> query, Guid animalId)
            => query.Where(x => x.BovineId == animalId);
    }
}