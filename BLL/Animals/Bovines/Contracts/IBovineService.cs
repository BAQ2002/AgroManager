using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;
using BLL.Animals.Contracts;

namespace BLL.Animals.Bovines.Contracts
{
    /// <summary>
    /// Contrato público da BLL para operações relacionadas a bovinos.
    /// </summary>
    public interface IBovineService : IAnimalService<BovineEntity>
    {
        // métodos exclusivos de bovino podem ser adicionados aqui futuramente
    }
}
