using BLL.Common.Contracts;
using MODEL;

namespace BLL.Services
{
    public interface IAnimalService<TAnimal> : ICrudService<TAnimal>
        where TAnimal : AnimalEntity
    {
        // Neste ponto, métodos comuns a qualquer animal podem ser adicionados.
        // Exemplo futuro:
        // void ChangeStatus(Guid id, AnimalStatus status);
    }
}