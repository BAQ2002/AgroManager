using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public abstract class WeightEntity : BaseEntity
    {
        private DateOnly _occurrenceDate;
        private float _weight;

        public DateOnly OccurrenceDate
        {
            get => _occurrenceDate;
            set => _occurrenceDate = value;
        }

        public float Weight
        {
            get => _weight;
            set
            {
                if (!float.IsFinite(value) || value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Weight must be finite and greater than zero.");

                _weight = value;
            }
        }
    }
}
