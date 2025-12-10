using Microsoft.EntityFrameworkCore;
using MxcEventManager.Models;

namespace MxcEventManager.Data;

public class SeedData
{
    public async Task SeedAsync(IServiceProvider services) 
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        // Seed Countries
        if (!await context.Countries.AnyAsync()) 
        {
            context.Countries.AddRange(
                new Country { Name = "Hungary" },
                new Country { Name = "Iceland" },
                new Country { Name = "Japan" }
            );

            await context.SaveChangesAsync();
        }

        // Seed locations
        if (!await context.Locations.AnyAsync())
        {
            var hungary = await context.Countries.FirstAsync(c => c.Name == "Hungary");

            var iceland = await context.Countries.FirstAsync(c => c.Name == "Iceland");

            var japan = context.Countries.FirstAsync(c => c.Name == "Japan");

            context.Locations.AddRange(
                new Location { Name = "Budapest", CountryId = hungary.Id },
                new Location { Name = "Reykjavik", CountryId = iceland.Id },
                new Location { Name = "Tokyo", CountryId = japan.Id }
            );

            await context.SaveChangesAsync();
        }

        // Seed events
        if (!await context.Events.AnyAsync())
        {
            var budapest = context.Locations.FirstAsync(l => l.Name == "Budapest");

            var reykjavik = context.Locations.FirstAsync(l => l.Name == "Reykjavik");

            var tokyo = context.Locations.FirstAsync(l => l.Name == "Tokyo");

            context.Events.AddRange(
                new Event { Name = "Conference 2025", LocationId = budapest.Id, Capacity = 250 },
                new Event { Name = "Local Workshop", LocationId = reykjavik.Id, Capacity = 50 },
                new Event { Name = "Tech Expo", LocationId = tokyo.Id, Capacity = 500 }
            );

            await context.SaveChangesAsync();
        }
    }
}
