using System;

namespace Fazendinha.Model
{
    #region INTERFACE--------------------------------------
    public interface IWeightTracker
    {
        DateOnly? LatestDate { get; }
        float? LatestWeight { get; }
        //IReadOnlyDictionary<DateOnly, float> History { get; }

        bool TryGetWeight(DateOnly date, out float weight);
        void Record(DateOnly date, float weight);
    }
    
    #endregion
        #region CLASS ----------------------------------------
    public class WeightTracker : IWeightTracker
    {
        private readonly Dictionary<DateOnly, float> _history = new();
        private DateOnly? _latestDate;
        private float? _latestWeight;



        public DateOnly? LatestDate { 
            get => _latestDate;
            protected set {_latestDate = value;}
        }

        public float? LatestWeight { 
            get => _latestWeight;
            protected set {_latestWeight = value;}
        }

        //public IReadOnlyDictionary<DateOnly, float> History
        //    => new ReadOnlyDictionary<DateOnly, float>(_history);


        #region CONSTRUCTORS -------------------------------------------
        public WeightTracker() { }

        public WeightTracker(DateOnly startDate, float startWeight)
        {
            Record(startDate, startWeight);
        }
        #endregion
        #region METHODS ----------------------------------------------
        public void Record(DateOnly date, float weight)
        {
            _history[date] = weight;      // grava o par recebido
            if (LatestDate is null || date >= LatestDate)
            {
                LatestDate = date;
                LatestWeight = weight;
            }
        }

        public bool TryGetWeight(DateOnly date, out float weight)
            => _history.TryGetValue(date, out weight);

        #endregion
    }
    #endregion
}
