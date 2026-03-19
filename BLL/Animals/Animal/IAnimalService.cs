using BLL.Common.Contracts;
using MODEL;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace BLL.Services
{
    public interface IAnimalService<TAnimal> : ICrudService<TAnimal>, IAnimalPhotoService<TAnimal>
        where TAnimal : AnimalEntity
    {
        Task<IReadOnlyList<TAnimal>> ListAsync(CancellationToken ct = default);

        /// <summary>
        /// Lista animais aplicando filtros comuns compartilhados entre espécies.
        /// </summary>
        /// <param name="filters">Modelo com critérios de consulta reutilizáveis.</param>
        /// <param name="ct">Token de cancelamento da operação assíncrona.</param>
        /// <returns>Coleção somente leitura com os animais filtrados.</returns>
        Task<IReadOnlyList<TAnimal>> ListAsync(AnimalFiltersModel filters, CancellationToken ct = default);

        // Neste ponto, métodos comuns a qualquer animal podem ser adicionados.
        // Exemplo futuro:
        // void ChangeStatus(Guid id, AnimalStatus status);
    }
}