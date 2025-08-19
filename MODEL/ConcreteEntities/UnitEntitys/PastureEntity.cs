using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public enum ForageGrassType
    {
        Unknown = 0,
        Tipo1 = 1,
        Tipo2 = 2,
        //Tipo3 = 3,
        //TipoEtc = etc
    }
    public enum SoilType
    {
        Unknown = 0,
        Tipo1 = 1,
        Tipo2 = 2,
        //Tipo3 = 3,
        //TipoEtc = etc
    }

    public class PastureEntity : BaseEntity
    {
        private float _area; //area em m2
        //private Dictionary<string, float> _areaMap;
        private Dictionary<float, SoilType> _areaMap;

    }
}
