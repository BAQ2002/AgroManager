using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class BovineEnumProperties
    {
        /// <summary>
        /// Define o estado de maternidade atual de uma vaca
        /// </summary>
        public enum MaritalStatus
        {
            Unknown = 0,
            Single = 1, //Vaca que nao esta com bezerro e nem gravida
            Pregnant = 2, //Vaca gravida
            Mother = 3 //Vaca com bezerro
        }
        /// <summary>
        /// Define se o bovino é de corte ou de leiteiro
        /// </summary>
        public enum CattleType
        {
            Unknown = 0,
            Beef = 1, //Gado de corte
            Dairy = 2, //Gado de leite
        }
    }
}
