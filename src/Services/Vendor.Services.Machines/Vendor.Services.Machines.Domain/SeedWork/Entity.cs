using System.ComponentModel.DataAnnotations;

namespace Vendor.Services.Machines.SeedWork;

public abstract class Entity
{
    [Key]
    public int Id { get; protected set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    public Entity()
    {
        CreatedOn = DateTime.Now;
        UpdatedOn = DateTime.Now;
    }
}