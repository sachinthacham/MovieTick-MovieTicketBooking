using MediatR;
using MovieBooking.Application.DTOs.Theaters;
using MovieBooking.Application.Features.Theaters.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Theaters.Handlers;

public class AddTheaterFacilityHandler : IRequestHandler<AddTheaterFacilityCommand, TheaterFacilityDto>
{
    private readonly ITheaterFacilityRepository _facilityRepository;
    private readonly ITheaterRepository _theaterRepository;

    public AddTheaterFacilityHandler(ITheaterFacilityRepository facilityRepository, ITheaterRepository theaterRepository)
    {
        _facilityRepository = facilityRepository;
        _theaterRepository = theaterRepository;
    }

    public async Task<TheaterFacilityDto> Handle(AddTheaterFacilityCommand request, CancellationToken cancellationToken)
    {
        var theater = await _theaterRepository.GetByIdAsync(request.TheaterId);
        if (theater == null) throw new KeyNotFoundException("Theater not found.");

        var facility = new TheaterFacility
        {
            TheaterId = request.TheaterId,
            FacilityName = request.FacilityName,
            Icon = request.Icon
        };

        await _facilityRepository.AddAsync(facility);

        return new TheaterFacilityDto
        {
            Id = facility.Id,
            FacilityName = facility.FacilityName,
            Icon = facility.Icon
        };
    }
}

public class RemoveTheaterFacilityHandler : IRequestHandler<RemoveTheaterFacilityCommand, bool>
{
    private readonly ITheaterFacilityRepository _facilityRepository;

    public RemoveTheaterFacilityHandler(ITheaterFacilityRepository facilityRepository)
    {
        _facilityRepository = facilityRepository;
    }

    public async Task<bool> Handle(RemoveTheaterFacilityCommand request, CancellationToken cancellationToken)
    {
        var facility = await _facilityRepository.GetByIdAsync(request.FacilityId);
        if (facility == null) throw new KeyNotFoundException("Facility not found.");

        await _facilityRepository.DeleteAsync(facility);
        return true;
    }
}
