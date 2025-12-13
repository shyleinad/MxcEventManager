using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MxcEventManager.Data;
using MxcEventManager.DTOs;
using MxcEventManager.Models;

namespace MxcEventManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<LocationController> logger;
    private const string locationNotFoundMessage = "Location not found!";

    public LocationController(AppDbContext db, ILogger<LocationController> logger)
    {
        dbContext = db;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetAll()
    {
        logger.LogInformation("Getting all locations");

        List<LocationDto> items = await dbContext.Locations
            .Include(l => l.Country)
            .AsNoTracking()
            .Select(l => MapHelper.MapModelToDto(l))
            .ToListAsync();

        logger.LogInformation("Got all locations.");

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LocationDto>> Get(int id)
    {
        logger.LogInformation("Getting location.");

        Location? locationModel = await dbContext
            .Locations
            .Include(l => l.Country)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (locationModel == null)
        {
            logger.LogWarning(locationNotFoundMessage);

            return NotFound();
        }

        LocationDto result = MapHelper.MapModelToDto(locationModel);

        logger.LogInformation("Got location");

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<LocationDto>> Create([FromBody] LocationDto locationDto)
    {
        logger.LogInformation("Creating location.");

        if (!ModelState.IsValid)
        {
            logger.LogError("Invalid model state!");

            return BadRequest(ModelState);
        }

        // if CountryId provided validate it
        if (locationDto.CountryId.HasValue)
        {
            var c = await dbContext
                .Countries
                .FindAsync(locationDto.CountryId.Value);

            if (c == null)
            {
                logger.LogError("Country id does not exist!");

                return BadRequest(new { error = "Country id does not exist." });
            }
        }

        Location? locationModel = MapHelper.MapDtoToModel(locationDto);

        dbContext.Locations.Add(locationModel);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Created location!");

        return CreatedAtAction(nameof(Get), new { id = locationModel.Id }, MapHelper.MapModelToDto(locationModel));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] LocationDto locationDto)
    {
        logger.LogInformation("Updating location.");

        if (!ModelState.IsValid)
        {
            logger.LogError("Invalid model state!");

            return BadRequest(ModelState);
        }

        Location? locationModel = await dbContext
            .Locations
            .FindAsync(id);

        if (locationModel == null)
        {
            logger.LogError(locationNotFoundMessage);

            return NotFound();
        }

        if (locationDto.CountryId.HasValue)
        {
            var c = await dbContext.Countries.FindAsync(locationDto.CountryId.Value);

            if (c == null)
            {
                logger.LogError("Country id does not found!");

                return BadRequest(new { error = "Country id does not exist." });
            }
        }

        locationModel.Name = locationDto.Name;
        locationModel.CountryId = locationDto.CountryId;

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Updated location.");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogInformation("Deleting location.");

        Location? locationModel = await dbContext
            .Locations
            .Include(l => l.Events)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (locationModel == null)
        {
            logger.LogError(locationNotFoundMessage);

            return NotFound();
        }

        if (locationModel.Events.Count == 0)
        {
            logger.LogError("Cannot delete location with events. Remove events first.");

            return BadRequest(new { error = "Cannot delete location with events. Remove events first." });
        }

        dbContext.Locations.Remove(locationModel);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Deleted location.");

        return NoContent();
    }
}
