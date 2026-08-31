using INFRA.Repositories.Weight;
using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA.Repositories.Milk
{
    public sealed class BovineMilkRepositoryEF : MilkRepositoryEFBase<BovineMilk>, IBovineMilkRepository
    {
        public BovineMilkRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) : base(factory){}

        protected override DbSet<BovineMilk> GetSet(AgroManagerDbContext db) => db.BovineMilkRecords;

        protected override IQueryable<BovineMilk> FilterByAnimalId(IQueryable<BovineMilk> query, Guid animalId)
            => query.Where(x => x.BovineId == animalId);
    } 
}
