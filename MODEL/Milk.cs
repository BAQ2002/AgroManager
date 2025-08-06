using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class Milk : BaseEntity
    {
        private Guid _animalId;
        private DateTimeOffset _occurrenceDate;
        private float _liters;
        
        public Guid AnimalId
        {
            get => _animalId;
            set { _animalId = value; }
        }
        public DateTimeOffset CccurrenceDate
        {
            get => _occurrenceDate;
            set { _occurrenceDate = value; }
        }
        public float Liters 
        {
            get => _liters;
            set { _liters = value; } 
        }
        //public string? propriedade1 { get; set; }
        //public string? propriedade2 { get; set; }
        //public string? propriedadeETC { get; set; }
    }
    public class MilkTrack
    {       
        private Guid _cowId;

        public MilkTrack(Guid cowId)
        {
            _cowId = cowId;
        }

        private Dictionary<DateOnly, Milk> _history = new();
        public void addRecord(DateOnly dateOnly, Milk milk)
        {
            _history [dateOnly] = milk;
        }


        
        public float CalcularMediaDiariaEntreDatas(Dictionary<DateOnly, float> historico, DateOnly data)
        {
            if (!historico.ContainsKey(data))
                return 0f;

            // Ordena as datas
            var datasOrdenadas = historico.Keys.OrderBy(d => d).ToList();
            int index = datasOrdenadas.IndexOf(data);

            // Se não há data anterior, retorna apenas o valor da data atual
            if (index <= 0)
                return historico[data];

            var dataAnterior = datasOrdenadas[index - 1];
            var diasIntervalo = data.DayNumber - dataAnterior.DayNumber;

            if (diasIntervalo <= 0)
                return historico[data]; // proteção contra erro de dados

            float valorAnterior = historico[dataAnterior];
            float valorAtual = historico[data];

            float somaPonderada = (valorAnterior * diasIntervalo); // assume constante até a nova data
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
