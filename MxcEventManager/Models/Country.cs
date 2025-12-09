using System.ComponentModel.DataAnnotations;

namespace MxcEventManager.Models;

public class Country
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public ICollection<Location> Locations { get; set; } = new List<Location>();
}