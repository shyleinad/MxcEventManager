using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MxcEventManager.Models;

public class Event
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public int LocationId { get; set; }

    [ForeignKey(nameof(LocationId))]
    public Location Location { get; set; }

    public int? Capacity { get; set; }
}