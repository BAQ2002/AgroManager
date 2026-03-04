using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL

{   /// <summary>
    /// Define o contrato genérico (port) de persistência de animais consumido pela camada de serviço.
    /// Cada implementação deve encapsular o mecanismo de acesso a dados e expor operações assíncronas
    /// para escrita e consulta de entidades derivadas de <see cref="AnimalEntity"/>.
    /// </summary>
    /// <typeparam name="TAnimal">
    /// Tipo da entidade animal. Deve herdar de AnimalEntity.
    /// </typeparam>
    public interface IAnimalRepository<TAnimal> where TAnimal : AnimalEntity
    {
        /// <summary>
        /// Persiste uma nova entidade animal no repositório.
        /// </summary>
        /// <param name="entity">Entidade a ser adicionada na origem de dados.</param>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        Task AddAsync(TAnimal entity, CancellationToken ct = default);

        /// <summary>
        /// Atualiza os dados de uma entidade animal já existente no repositório.
        /// </summary>
        /// <param name="entity">Entidade com os novos valores que devem ser persistidos.</param>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        Task UpdateAsync(TAnimal entity, CancellationToken ct = default);

        /// <summary>
        /// Remove uma entidade animal previamente carregada da origem de dados.
        /// </summary>
        /// <param name="entity">Entidade que será removida.</param>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        Task DeleteAsync(TAnimal entity, CancellationToken ct = default);

        /// <summary>
        /// Busca uma entidade animal pelo identificador único.
        /// </summary>
        /// <param name="id">Identificador da entidade.</param>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        /// <returns>
        /// A entidade encontrada ou <see langword="null"/> quando não existir registro para o identificador informado.
        /// </returns>
        Task<TAnimal?> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Busca uma entidade animal pelo nome.
        /// </summary>
        /// <param name="name">Nome utilizado como critério da busca.</param>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        /// <returns>
        /// A entidade encontrada ou <see langword="null"/> quando não houver correspondência exata para o nome informado.
        /// </returns>
        Task<TAnimal?> GetByNameAsync(string name, CancellationToken ct = default);

        /// <summary>
        /// Lista todas as entidades animais disponíveis no repositório.
        /// </summary>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        /// <returns>Coleção somente leitura contendo os animais encontrados.</returns>
        public Task<IReadOnlyList<TAnimal>> ListAsync(CancellationToken ct = default);

        /// <summary>
        /// Lista entidades animais aplicando filtros comuns reaproveitáveis por qualquer espécie.
        /// Esse contrato substitui a necessidade de filtros específicos por entidade quando os critérios
        /// pertencem ao núcleo comum de <see cref="AnimalEntity"/>.
        /// </summary>
        /// <param name="filters">Modelo de filtros comuns para consulta de animais.</param>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        /// <returns>Coleção somente leitura com os animais que atenderam aos critérios.</returns>
        Task<IReadOnlyList<TAnimal>> ListAsync(AnimalFiltersModel filters, CancellationToken ct = default);

        /// <summary>
        /// Busca uma entidade animal pelo gênero representado no enum <see cref="Gender"/>.
        /// </summary>
        /// <param name="gender">Valor de gênero usado no filtro.</param>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        /// <returns>
        /// A entidade encontrada ou <see langword="null"/> quando não houver registro com o gênero informado.
        /// </returns>
        Task<TAnimal?> GetByGenderAsync(Gender gender, CancellationToken ct = default);

        /// <summary>
        /// Busca uma entidade animal pelo gênero informado em representação numérica.
        /// </summary>
        /// <param name="gender">Valor inteiro equivalente ao enum <see cref="Gender"/>.</param>
        /// <param name="ct">Token utilizado para cancelar a operação assíncrona.</param>
        /// <returns>
        /// A entidade encontrada ou <see langword="null"/> quando não houver registro com o gênero informado.
        /// </returns>
        Task<TAnimal?> GetByGenderAsync(int gender, CancellationToken ct = default);
    }
}
