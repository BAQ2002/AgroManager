using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INFRA
{
    /// <summary>
    /// Classe que implementa os métodos de interação com o banco de dados
    /// </summary>
    public sealed class SwineRepositoryEF : IAnimalRepository<SwineEntity>
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public SwineRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        public override async Task AddAsync(SwineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Swines.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public override async Task UpdateAsync(SwineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);

            db.Swines.Update(entity);       // marca como Modified
            await db.SaveChangesAsync(ct);
        }

        public override async Task DeleteAsync(SwineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Swines.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public override async Task<IReadOnlyList<SwineEntity>> ListAsync(CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().ToListAsync(ct);
        }


        /// <summary>
        /// Retorna suínos aplicando filtros comuns compartilhados entre espécies por meio de <see cref="AnimalFiltersModel"/>.
        /// </summary>
        /// <param name="filters">Filtros comuns aplicáveis ao domínio base de animais.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        /// <returns>Lista somente leitura com os suínos filtrados.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="filters"/> é nulo.</exception>
        public override async Task<IReadOnlyList<SwineEntity>> ListAsync(AnimalFiltersModel filters, CancellationToken ct = default)
        {
            if (filters is null) throw new ArgumentNullException(nameof(filters));

            await using var db = await _factory.CreateDbContextAsync(ct);

            IQueryable<SwineEntity> query = db.Swines.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filters.Name))
            {
                string normalizedName = filters.Name.Trim();
                query = query.Where(x => x.Name != null && x.Name.Contains(normalizedName));
            }

            if (filters.Origin.HasValue)
            {
                query = query.Where(x => x.Origin == filters.Origin.Value);
            }

            if (filters.Gender.HasValue)
            {
                query = query.Where(x => x.Gender == filters.Gender.Value);
            }

            if (filters.BirthDateFrom.HasValue)
            {
                query = query.Where(x => x.BirthDate.HasValue && x.BirthDate.Value >= filters.BirthDateFrom.Value);
            }

            if (filters.BirthDateTo.HasValue)
            {
                query = query.Where(x => x.BirthDate.HasValue && x.BirthDate.Value <= filters.BirthDateTo.Value);
            }

            if (filters.Skip.HasValue && filters.Skip.Value > 0)
            {
                query = query.Skip(filters.Skip.Value);
            }

            if (filters.Take.HasValue && filters.Take.Value > 0)
            {
                query = query.Take(filters.Take.Value);
            }

            return await query.ToListAsync(ct);
        }

        public override async Task<SwineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }

        public override async Task<SwineEntity?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name, ct);
        }

        public override async Task<SwineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().SingleOrDefaultAsync(x => x.Gender == gender, ct);
        }

        public override async Task<SwineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().SingleOrDefaultAsync(x => (int)x.Gender == gender, ct);
        }
    }
}
