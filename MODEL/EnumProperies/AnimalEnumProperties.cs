using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public enum Gender
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }

    /// <summary>
    /// Define se o animal foi comprado ou nascido ou se nao pertence a fazenda
    /// </summary>
    public enum AcquisitionOrigin
    {
        Unknown = 0, //Desconhecido 
        Born = 1, //Nascido na fazenda 
        Purchased = 2, //Comprado
        External = 3 //Nao faz parte da fazenda
    }

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
}
