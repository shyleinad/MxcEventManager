using System.ComponentModel.DataAnnotations;

namespace MxcEventManager.DTOs;

public class LocationDto
{
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    public int? CountryId { get; set; }

    public string? CountryName { get; set; }
}
