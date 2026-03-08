using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    #region Animal Enum Properties
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

    public enum AgeUnit
    {
        Days = 0,
        Months = 1,
        Years = 2
    }

    #endregion

    #region Bonive Enum Properties
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
    #endregion

    #region Swine Enum Properties
    /// <summary>
    /// Define se o porco é de corte ou matriz
    /// </summary>
    public enum PorcType
    {
        Unknown = 0,
        Beef = 1, //Porco de corte
        Breeder = 2, //Porco matriz
    }
    #endregion
}
