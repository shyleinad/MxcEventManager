using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MxcEventManager.Data;
using MxcEventManager.DTOs;
using MxcEventManager.Models;

namespace MxcEventManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<CountryController> logger;

    public CountryController(AppDbContext db, ILogger<CountryController> logger)
    {
        dbContext = db;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll()
    {
        logger.LogInformation("Getting all countries.");

        List<CountryDto> items = await dbContext
            .Countries
            .Select(c => MapHelper.MapModelToDto(c))
            .ToListAsync();

        logger.LogInformation("Got {0} countries.", items.Count);

        return Ok(items);
    }

    [HttpGet]
    public async Task<ActionResult<CountryDto>> Get(int id)
    {
        logger.LogInformation("Getting country.");

        Country? countryModel = await dbContext
            .Countries
            .FirstOrDefaultAsync(c => c.Id == id);

        if (countryModel == null)
        {
            logger.LogWarning("Country not found!");

            return NotFound();
        }

        CountryDto result = MapHelper.MapModelToDto(countryModel);

        logger.LogInformation("Got country.");

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<CountryDto>> Create([FromBody] CountryDto countryDto)
    {
        logger.LogInformation("Creating country.");

        if (!ModelState.IsValid)
        {
            logger.LogError("Invalid model state!");

            return BadRequest(ModelState);
        }

        Country? countryModel = MapHelper.MapDtoToModel(countryDto);

        dbContext.Countries.Add(countryModel);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Created country.");

        return CreatedAtAction(nameof(GetAll), new { id = countryModel.Id }, countryDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CountryDto countryDto)
    {
        logger.LogInformation("Updating country.");

        if (!ModelState.IsValid)
        {
            logger.LogError("Invalid model state!");

            return BadRequest(ModelState);
        }

        Country? countryModel = await dbContext
            .Countries
            .FindAsync(id);

        if (countryModel == null)
        {
            logger.LogError("Country not found!");

            return NotFound();
        }

        countryModel.Name = countryDto.Name;

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Updated country.");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogInformation("Deleting country.");

        Country? countryModel = await dbContext
            .Countries
            .Include(c => c.Locations)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (countryModel == null)
        {
            logger.LogError("Country not found!");

            return NotFound();
        }

        if (countryModel.Locations.Any())
        {
            logger.LogError("Cannot delete country that has locations. Remove or reassign locations first.");

            return BadRequest(new { error = "Cannot delete country that has locations. Remove or reassign locations first." });
        }

        dbContext.Countries.Remove(countryModel);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Deleted country.");

        return NoContent();
    }
}
