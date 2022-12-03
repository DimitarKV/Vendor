using System.ComponentModel.DataAnnotations;

namespace Vendor.Domain.Entities;

public class Entity<T> : IEntity<T>
{
    [Key()]
    public T Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    public Entity()
    {
        CreatedOn = DateTime.Now;
        UpdatedOn = DateTime.Now;
    }
}