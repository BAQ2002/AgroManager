using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Exceptions
{
    /// <summary>
    /// Exceção destinada a violações de regras de negócio (domínio).
    /// Deve ser capturada pelo PL para exibir mensagem amigável ao usuário.
    /// </summary>
    public sealed class BusinessRuleException : Exception
    {
        /// <summary>
        /// Inicializa uma nova instância de BusinessRuleException com a mensagem informada.
        /// </summary>
        /// <param name="message">Mensagem de erro de negócio.</param>
        public BusinessRuleException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância de BusinessRuleException com mensagem e exceção interna.
        /// </summary>
        /// <param name="message">Mensagem de erro de negócio.</param>
        /// <param name="innerException">Exceção interna.</param>
        public BusinessRuleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
