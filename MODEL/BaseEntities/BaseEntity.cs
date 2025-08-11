namespace MODEL
{
    /// <summary>
    /// Classe que qualquer entidade registravel deve ser derivada
    /// </summary>
    public abstract class BaseEntity
    {
        private Guid _id;
        private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;
        private DateTimeOffset? _updatedAt = DateTimeOffset.UtcNow;

        public Guid Id 
        {
            get => _id;
            set { _id = value;}
        }

        public DateTimeOffset CreatedAt 
        { 
            get => _createdAt;
            set { _createdAt = value; } 
        } 

        public DateTimeOffset? UpdatedAt
        { 
            get => _updatedAt; 
            set { _updatedAt = value; } 
        }
        
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        protected virtual void Update()
        {             
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        

    }

}