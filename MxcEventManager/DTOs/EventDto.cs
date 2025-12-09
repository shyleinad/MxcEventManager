using System.ComponentModel.DataAnnotations;

namespace MxcEventManager.DTOs;

public class EventDto
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public int LocationId { get; set; }

    public string? LocationName { get; set; }

    public string? CountryName { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
    public int? Capacity { get; set; }
}
