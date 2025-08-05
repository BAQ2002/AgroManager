using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    
    public class BovineAnimalEntity : AnimalEntity
    {
        private MaritalStatus? _matrialStatus = null;

        public MaritalStatus? MatrialStatus 
        {  
            get =>_matrialStatus;  
            set { _matrialStatus = value; } 
        }

        private float? _milkProduction = null;
        public BovineAnimalEntity() 
        {
            
        }

    }
}
