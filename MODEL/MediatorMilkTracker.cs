using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class MilkItem
    {
        public float Liters { get; set; }
        public string TestProp { get; set; }
        public MilkItem(MilkEntity milkEntity) 
        {
            Liters = milkEntity.Liters;
            TestProp = milkEntity.TestProp;
        }

        public MilkItem(float liters, string testProp) 
        {         
            Liters = liters;
            TestProp = testProp;
        }

    }
    public class MediatorMilkTracker
    {
        private Guid _cowId;
        private Dictionary<DateOnly, MilkItem> _milkHistory = new();

        public MediatorMilkTracker(BovineAnimalEntity animalEntity, MilkEntity[] milkEntity)
        {
            _cowId = animalEntity.Id;

            for (int i = 0; i < milkEntity.Length; i++)
            {
                _milkHistory[milkEntity[i].OccurrenceDate] = new MilkItem(milkEntity[i]);
            }
        }
        /// <summary>
        /// Retorna a media de producao de leite entre a DateOnly passada como parametro e o registro anterior a ela
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public float GetDailyAverageLiters(DateOnly date)
        {
            if (!_milkHistory.ContainsKey(date))
                return 0f;

            // Ordena as datas
            var datasOrdenadas = _milkHistory.Keys.OrderBy(d => d).ToList();
            int index = datasOrdenadas.IndexOf(date); //indice da date de parametro           

            // Se não há date anterior, retorna apenas o valor da date atual
            if (index <= 0)
                return _milkHistory[date].Liters;

            var previousDate = datasOrdenadas[index - 1];
            var diasIntervalo = date.DayNumber - previousDate.DayNumber;

            if (diasIntervalo <= 0)
                return _milkHistory[date].Liters; // proteção contra erro de dados

            float currentValue = _milkHistory[date].Liters;
            float previousValue = _milkHistory[previousDate].Liters;

            float previousValueWeight = (previousValue - currentValue) / diasIntervalo;
            for(int i =0; i <= diasIntervalo; i++)
            float currentValueWeight =

            float somaPonderada = (previousValue * diasIntervalo); // assume constante até a nova date
            return somaPonderada / diasIntervalo;
        }
        public float CalcularMediaSemanalEntreDatas(Dictionary<DateOnly, float> historico, DateOnly data)
        {
            if (!historico.ContainsKey(data))
                return 0f;

            var datasOrdenadas = historico.Keys.OrderBy(d => d).ToList();
            int index = datasOrdenadas.IndexOf(data);

            if (index <= 0)
                return historico[data];

            var dataAnterior = datasOrdenadas[index - 1];
            int diasIntervalo = data.DayNumber - dataAnterior.DayNumber;

            if (diasIntervalo <= 0)
                return historico[data];

            float valorAnterior = historico[dataAnterior];
            float somaPonderada = valorAnterior * diasIntervalo;

            // Normaliza pela duração de uma semana
            return somaPonderada / 7f;
        }
        public float CalcularMediaSemanalEntreDatas(Dictionary<DateOnly, float> historico, DateOnly data)
        {
            if (!historico.ContainsKey(data))
                return 0f;

            var datasOrdenadas = historico.Keys.OrderBy(d => d).ToList();
            int index = datasOrdenadas.IndexOf(data);

            if (index <= 0)
                return historico[data];

            var dataAnterior = datasOrdenadas[index - 1];
            int diasIntervalo = data.DayNumber - dataAnterior.DayNumber;

            if (diasIntervalo <= 0)
                return historico[data];

            float valorAnterior = historico[dataAnterior];
            float somaPonderada = valorAnterior * diasIntervalo;

            // Normaliza pela duração de uma semana
            return somaPonderada / 7f;
        }

    }
}
