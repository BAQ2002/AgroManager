using Microsoft.Win32;
using System;

namespace MODEL
{
    public class LoteRegister: BaseEntity
    {
        public Guid LoteId { get; set; }
        public Guid animalId { get; set; }
        public DateTimeOffset? dataEntrada { get; set; }
        public DateTimeOffset? dataSaida { get; set; }
        public string? motivoSaida { get; set; }

        public LoteRegister()
        { 
       
        }
   
    }

    public abstract class Lote : BaseEntity
    {
        private string _name { get; set; } = null!;

        private List<LoteRegister> _animalsList = new();

        public Lote()
        { 

        }


        // Histórico de permanências de animais no lote
        public ICollection<AnimalLote> Alocacoes { get; } = new HashSet<AnimalLote>();

        // Animais presentes neste momento
        public IEnumerable<Animal> AnimaisAtivos =>
            Alocacoes.Where(a => a.DataSaida == null).Select(a => a.Animal);
    }
    
}