using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{   
    public class BovineEntity : AnimalEntity
    {
        private MaritalStatus? _matrialStatus = null;
        private CattleType? _cattleType = null;
        public MaritalStatus? MaritalStatus 
        {  
            get =>_matrialStatus;  
            set { _matrialStatus = value; } 
        }
     
        public CattleType? CattleType
        {
            get => _cattleType;
            set { _cattleType = value; }
        }

        public BovineEntity() 
        {
            
        }

    }
}
