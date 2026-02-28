using BLL.Common.Contracts;
using MODEL;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace BLL.Services
{
    public interface IAnimalService<TAnimal> : ICrudService<TAnimal>
        where TAnimal : AnimalEntity
    {
        Task<IReadOnlyList<TAnimal>> ListAsync(CancellationToken ct = default);

        // Neste ponto, métodos comuns a qualquer animal podem ser adicionados.
        // Exemplo futuro:
        // void ChangeStatus(Guid id, AnimalStatus status);
    }
}