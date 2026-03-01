using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace INFRA
{
    /// <summary>
    /// Implementação EF Core do contrato <see cref="IAnimalRepository{TAnimal}"/> para <see cref="BovineEntity"/>.
    /// Cada operação cria um <see cref="AgroManagerDbContext"/> via <see cref="IDbContextFactory{TContext}"/>
    /// para isolar o ciclo de vida da transação e persistir/consultar dados de bovinos.
    /// </summary>
    public sealed class BovineRepositoryEF : IAnimalRepository<BovineEntity>
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;

        /// <summary>
        /// Inicializa o repositório com a fábrica de contexto utilizada para criação sob demanda do DbContext.
        /// </summary>
        /// <param name="factory">Fábrica de <see cref="AgroManagerDbContext"/> usada em todas as operações.</param>
        public BovineRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/>, adiciona a entidade em <see cref="AgroManagerDbContext.Bovines"/>
        /// com <c>Add</c> e confirma a transação com <c>SaveChangesAsync</c>.
        /// </summary>
        /// <param name="entity">Entidade bovina a ser persistida.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        public override async Task AddAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Bovines.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/>, marca a entidade como modificada em
        /// <see cref="AgroManagerDbContext.Bovines"/> via <c>Update</c> e salva as alterações com <c>SaveChangesAsync</c>.
        /// </summary>
        /// <param name="entity">Entidade bovina contendo os novos valores.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        public override async Task UpdateAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);

            db.Bovines.Update(entity);
            await db.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/>, remove a entidade do conjunto
        /// <see cref="AgroManagerDbContext.Bovines"/> com <c>Remove</c> e efetiva a exclusão com <c>SaveChangesAsync</c>.
        /// </summary>
        /// <param name="entity">Entidade bovina a ser removida.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        public override async Task DeleteAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Bovines.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/> e retorna todos os bovinos sem rastreamento de alterações,
        /// utilizando <c>AsNoTracking</c> seguido de <c>ToListAsync</c>.
        /// </summary>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        /// <returns>Lista somente leitura com os bovinos encontrados.</returns>
        public override async Task<IReadOnlyList<BovineEntity>> ListAsync(CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().ToListAsync(ct);
        }


        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/> e retorna bovinos aplicando filtros comuns de <see cref="AnimalFiltersModel"/>.
        /// O método compõe a consulta dinamicamente e aplica paginação opcional via <c>Skip</c> e <c>Take</c>.
        /// </summary>
        /// <param name="filters">Filtros comuns aplicáveis a qualquer entidade de animal.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        /// <returns>Lista somente leitura com os bovinos filtrados.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="filters"/> é nulo.</exception>
        public override async Task<IReadOnlyList<BovineEntity>> ListAsync(AnimalFiltersModel filters, CancellationToken ct = default)
        {
            if (filters is null) throw new ArgumentNullException(nameof(filters));

            await using var db = await _factory.CreateDbContextAsync(ct);

            IQueryable<BovineEntity> query = db.Bovines.AsNoTracking();

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
        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/> e consulta um bovino por identificador
        /// usando <c>AsNoTracking</c> e <c>SingleOrDefaultAsync</c>.
        /// </summary>
        /// <param name="id">Identificador do bovino.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        /// <returns>A entidade encontrada ou <see langword="null"/>.</returns>
        public override async Task<BovineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }

        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/> e consulta um bovino por nome
        /// usando <c>AsNoTracking</c> e <c>SingleOrDefaultAsync</c>.
        /// </summary>
        /// <param name="name">Nome utilizado como filtro da consulta.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        /// <returns>A entidade encontrada ou <see langword="null"/>.</returns>
        public override async Task<BovineEntity?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name, ct);
        }

        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/> e consulta um bovino por gênero enum
        /// com <c>AsNoTracking</c> e <c>SingleOrDefaultAsync</c>.
        /// </summary>
        /// <param name="gender">Gênero utilizado no filtro.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        /// <returns>A entidade encontrada ou <see langword="null"/>.</returns>
        public override async Task<BovineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Gender == gender, ct);
        }

        /// <summary>
        /// Cria um <see cref="AgroManagerDbContext"/> e consulta um bovino por gênero numérico,
        /// convertendo o enum para inteiro no predicado de <c>SingleOrDefaultAsync</c>.
        /// </summary>
        /// <param name="gender">Valor inteiro do gênero utilizado no filtro.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        /// <returns>A entidade encontrada ou <see langword="null"/>.</returns>
        public override async Task<BovineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => (int)x.Gender == gender, ct);
        }
    }
}
