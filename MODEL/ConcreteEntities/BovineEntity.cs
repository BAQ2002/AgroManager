using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public interface IBovineRepository
    {
        Task AddAsync(BovineEntity entity, CancellationToken ct = default);
        Task<BovineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BovineEntity?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<BovineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default);
        Task<BovineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default);
    }

    public class BovineEntity : AnimalEntity
    {
        private MaritalStatus? _matrialStatus = null;
        private CattleType? _cattleType = null;
        public MaritalStatus? MaritalStatus 
        {  
            get =>_matrialStatus;  
            set { _matrialStatus = value; } 
        }
     
        public CattleType? CattleType
        {
            get => _cattleType;
            set { _cattleType = value; }
        }
        public BovineEntity() 
        {
            
        }

    }
}
