using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    /// <summary>
    /// Species-independent base for milk measurements.
    /// The concrete species entity owns the animal foreign key.
    /// </summary>
    public abstract class MilkEntity : BaseEntity
    {
        private DateOnly _occurrenceDate;
        private float _liters;

        public DateOnly OccurrenceDate
        {
            get => _occurrenceDate;
            set { _occurrenceDate = value; }
        }
        public float Liters
        {
            get => _liters;
            set
            {
                if (!float.IsFinite(value) || value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Liters must be a finite value greater than zero.");

                _liters = value;
            }
        }
    }
}
