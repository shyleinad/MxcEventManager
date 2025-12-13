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
#pragma warning disable CS8618
    public Location Location { get; set; }
#pragma warning restore CS8618

    public int? Capacity { get; set; }
}