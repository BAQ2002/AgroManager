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
        private DateTimeOffset _occurredAt;
        private float _liters;

        public DateTimeOffset OccurredAt
        {
            get => _occurredAt;
            set { _occurredAt = value; }
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
