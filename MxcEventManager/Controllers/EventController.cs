using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MxcEventManager.Data;
using MxcEventManager.DTOs;
using MxcEventManager.Models;

namespace MxcEventManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly AppDbContext dbContext;

    private readonly ILogger<EventController> logger;

    public EventController(AppDbContext dbContext, ILogger<EventController> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<EventDto>>> GetAll()
    {
        logger.LogInformation("Getting all events.");

        List<EventDto> result = await dbContext.Events
            .Include(e => e.Location)
            .ThenInclude(l => l.Country)
            .Select(e => MapHelper.MapModelToDto(e))
            .ToListAsync();

        logger.LogInformation("Got {0} events.", result.Count);

        return result;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDto>> Get(int id)
    {
        logger.LogInformation("Getting event.");

        Event? eventModel = await dbContext.Events
            .Include(e => e.Location)
            .ThenInclude(l => l.Country)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventModel is null) 
        {
            logger.LogWarning("Event not found!");

            return NotFound();
        }

        EventDto result = MapHelper.MapModelToDto(eventModel);

        logger.LogInformation("Got event.");

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> Create([FromBody] EventDto eventDto)
    {
        logger.LogInformation("Creating event.");

        if (!ModelState.IsValid)
        {
            logger.LogError("Invalid model state!");

            return BadRequest(ModelState);
        }

        Event eventModel = MapHelper.MapDtoToModel(eventDto);

        dbContext.Events.Add(eventModel);

        await dbContext.SaveChangesAsync();

        eventDto.Id = eventModel.Id;

        logger.LogInformation("Created country.");

        return CreatedAtAction(nameof(Get), new { id = eventDto.Id }, eventDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] EventDto eventDto)
    {
        logger.LogInformation("Updating event.");

        if (!ModelState.IsValid) 
        {
            logger.LogError("Invalid model state!");

            return BadRequest(ModelState); 
        }

        Event? eventModel = await dbContext.Events.FindAsync(id);

        if (eventModel == null)
        {
            logger.LogError("Event not found!");

            return NotFound();
        }

        Location? locationModel = await dbContext.Locations.FindAsync(eventDto.LocationId);

        if (locationModel == null)
        {
            logger.LogError("Location not found!");

            return BadRequest(new { error = "Event id does not exist." });
        }

        eventModel.Name = eventDto.Name;
        eventModel.LocationId = eventDto.LocationId;
        eventModel.Capacity = eventDto.Capacity;

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Updated event.");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogInformation("Deleting event.");

        Event? eventModel = await dbContext.Events.FindAsync(id);

        if (eventModel == null)
        {
            logger.LogError("Event not found!");

            return NotFound();
        }

        dbContext.Events.Remove(eventModel);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Deleted event.");

        return NoContent();
    }
}