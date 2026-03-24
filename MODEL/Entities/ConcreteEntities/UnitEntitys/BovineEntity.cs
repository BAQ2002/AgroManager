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
        private BovineBreed? _breed = null;

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
        public BovineBreed? Breed
        {
            get => _breed;
            set { _breed = value; }
        }

        public BovineEntity() 
        {
            
        }

    }
}
