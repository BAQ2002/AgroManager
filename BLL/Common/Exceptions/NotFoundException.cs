using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Exceptions
{
    /// <summary>
    /// Exceção destinada a indicar que um registro/entidade não foi encontrado.
    /// </summary>
    public sealed class NotFoundException : Exception
    {
        /// <summary>
        /// Inicializa uma nova instância de NotFoundException com a mensagem informada.
        /// </summary>
        /// <param name="message">Mensagem de "não encontrado".</param>
        public NotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância de NotFoundException com mensagem e exceção interna.
        /// </summary>
        /// <param name="message">Mensagem de "não encontrado".</param>
        /// <param name="innerException">Exceção interna.</param>
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
