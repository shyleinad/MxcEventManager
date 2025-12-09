using System.ComponentModel.DataAnnotations;

namespace MxcEventManager.DTOs;

public class CountryDto
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }
}
