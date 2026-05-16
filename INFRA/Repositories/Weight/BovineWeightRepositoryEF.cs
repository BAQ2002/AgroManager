using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Repositories.Weight
{
    using Microsoft.EntityFrameworkCore;
    using MODEL;

    public sealed class BovineWeightRepositoryEF : AnimalWeightRepositoryEFBase<BovineWeight> , IBovineWeightRepository
    {
        public BovineWeightRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) : base(factory) { }

        protected override DbSet<BovineWeight> GetDbSet(AgroManagerDbContext db) => db.BovineWeightRecords;

        protected override IQueryable<BovineWeight> GetByAnimalId(IQueryable<BovineWeight> query, Guid animalId)
            => query.Where(x => x.BovineId == animalId);
    }
}