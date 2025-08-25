using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class MilkEntity : BaseEntity
    {
        private Guid _animalId;
        private DateOnly _occurrenceDate;
        private float _liters;

        public Guid BovineId
        {
            get => _animalId;
            set { _animalId = value; }
        }
        public DateOnly OccurrenceDate
        {
            get => _occurrenceDate;
            set { _occurrenceDate = value; }
        }
        public float Liters 
        {
            get => _liters;
            set { _liters = value; } 
        }

    }
    public class MilkTrack
    {       
        
    }
}
