using Microsoft.Win32;
using System;
using System.Security.Cryptography;

namespace MODEL
{
    
    public abstract class BatchEntity : BaseEntity
    {
        private string _name;
        private string? _description;

        public string Name
        {
            get => _name;
            set { _name = value; }
        }

        public string? Description
        {
            get => _description;
            set { _description = value; }
        }

        public List<BatchAnimalEntity> AnimalsList = new();

        public BatchEntity()
        { 

        }


        // Histórico de permanências de animais no lote
        //public ICollection<AnimalLote> Alocacoes { get; } = new HashSet<AnimalLote>();

        // Animais presentes neste momento
        //public IEnumerable<Animal> AnimaisAtivos =>
          //  Alocacoes.Where(a => a.DataSaida == null).Select(a => a.Animal);
    }
    
}