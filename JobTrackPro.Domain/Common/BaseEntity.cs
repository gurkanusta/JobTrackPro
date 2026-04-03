

namespace JobTrackPro.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();  
    public bool IsDeleted { get; protected set; }    
    public DateTime? DeletedAt { get; protected set; }
}