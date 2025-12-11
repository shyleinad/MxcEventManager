using MxcEventManager.DTOs;
using MxcEventManager.Models;

namespace MxcEventManager.Controllers;

public static class MapHelper
{
    public static CountryDto MapModelToDto(Country countryModel)
    {
        CountryDto result = new CountryDto
        {
            Id = countryModel.Id,
            Name = countryModel.Name
        };

        return result;
    }

    public static LocationDto MapModelToDto(Location locationModel)
    {
        LocationDto result = new LocationDto
        {
            Id = locationModel.Id,
            Name = locationModel.Name,
            CountryId = locationModel.CountryId,
            CountryName = locationModel.Country?.Name
        };

        return result;
    }

    public static EventDto MapModelToDto(Event eventModel)
    {
        EventDto result = new EventDto
        {
            Id = eventModel.Id,
            Name = eventModel.Name,
            LocationId = eventModel.LocationId,
            LocationName = eventModel.Location.Name,
            CountryName = eventModel.Location.Country?.Name,
            Capacity = eventModel.Capacity
        };

        return result;
    }

    public static Country MapDtoToModel(CountryDto countryDto)
    {
        Country result = new Country
        {
            Id = countryDto.Id,
            Name = countryDto.Name
        };

        return result;
    }

    public static Location MapDtoToModel(LocationDto locationDto)
    {
        Location result = new Location
        {
            Id = locationDto.Id,
            Name = locationDto.Name,
            CountryId = locationDto.CountryId,
        };

        return result;
    }

    public static Event MapDtoToModel(EventDto eventDto)
    {
        Event result = new Event
        {
            Id = eventDto.Id,
            Name = eventDto.Name,
            LocationId = eventDto.LocationId,
            Capacity = eventDto.Capacity
        };

        return result;
    }
}
