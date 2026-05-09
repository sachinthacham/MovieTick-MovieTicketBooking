using MediatR;
using MovieBooking.Application.DTOs.Theaters;
using MovieBooking.Application.Features.Theaters.Commands;
using MovieBooking.Application.Features.Theaters.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Theaters.Handlers;

public class CreateTheaterHandler : IRequestHandler<CreateTheaterCommand, TheaterDto>
{
    private readonly ITheaterRepository _theaterRepository;

    public CreateTheaterHandler(ITheaterRepository theaterRepository)
    {
        _theaterRepository = theaterRepository;
    }

    public async Task<TheaterDto> Handle(CreateTheaterCommand request, CancellationToken cancellationToken)
    {
        var theater = new Theater
        {
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Country = request.Country,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        await _theaterRepository.AddAsync(theater);
        return TheaterMappingHelper.ToDto(theater);
    }
}

public class UpdateTheaterHandler : IRequestHandler<UpdateTheaterCommand, TheaterDto>
{
    private readonly ITheaterRepository _theaterRepository;

    public UpdateTheaterHandler(ITheaterRepository theaterRepository)
    {
        _theaterRepository = theaterRepository;
    }

    public async Task<TheaterDto> Handle(UpdateTheaterCommand request, CancellationToken cancellationToken)
    {
        var theater = await _theaterRepository.GetByIdAsync(request.Id);
        if (theater == null) throw new KeyNotFoundException("Theater not found.");

        if (!string.IsNullOrWhiteSpace(request.Name)) theater.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.Description)) theater.Description = request.Description;
        if (!string.IsNullOrWhiteSpace(request.Address)) theater.Address = request.Address;
        if (!string.IsNullOrWhiteSpace(request.City)) theater.City = request.City;
        if (!string.IsNullOrWhiteSpace(request.State)) theater.State = request.State;
        if (!string.IsNullOrWhiteSpace(request.Country)) theater.Country = request.Country;
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber)) theater.PhoneNumber = request.PhoneNumber;
        if (!string.IsNullOrWhiteSpace(request.Email)) theater.Email = request.Email;
        if (request.Latitude.HasValue) theater.Latitude = request.Latitude.Value;
        if (request.Longitude.HasValue) theater.Longitude = request.Longitude.Value;
        if (request.IsActive.HasValue) theater.IsActive = request.IsActive.Value;
        theater.UpdatedAt = DateTime.UtcNow;

        await _theaterRepository.UpdateAsync(theater);
        return TheaterMappingHelper.ToDto(theater);
    }
}

public class DeleteTheaterHandler : IRequestHandler<DeleteTheaterCommand, bool>
{
    private readonly ITheaterRepository _theaterRepository;

    public DeleteTheaterHandler(ITheaterRepository theaterRepository)
    {
        _theaterRepository = theaterRepository;
    }

    public async Task<bool> Handle(DeleteTheaterCommand request, CancellationToken cancellationToken)
    {
        var theater = await _theaterRepository.GetByIdAsync(request.Id);
        if (theater == null) throw new KeyNotFoundException("Theater not found.");

        await _theaterRepository.DeleteAsync(theater);
        return true;
    }
}

public class GetAllTheatersHandler : IRequestHandler<GetAllTheatersQuery, List<TheaterDto>>
{
    private readonly ITheaterRepository _theaterRepository;

    public GetAllTheatersHandler(ITheaterRepository theaterRepository)
    {
        _theaterRepository = theaterRepository;
    }

    public async Task<List<TheaterDto>> Handle(GetAllTheatersQuery request, CancellationToken cancellationToken)
    {
        var theaters = await _theaterRepository.GetAllAsync(request.City, request.IsActive);
        return theaters.Select(TheaterMappingHelper.ToDto).ToList();
    }
}

public class GetTheaterByIdHandler : IRequestHandler<GetTheaterByIdQuery, TheaterDto>
{
    private readonly ITheaterRepository _theaterRepository;

    public GetTheaterByIdHandler(ITheaterRepository theaterRepository)
    {
        _theaterRepository = theaterRepository;
    }

    public async Task<TheaterDto> Handle(GetTheaterByIdQuery request, CancellationToken cancellationToken)
    {
        var theater = await _theaterRepository.GetByIdAsync(request.Id);
        if (theater == null) throw new KeyNotFoundException("Theater not found.");

        return TheaterMappingHelper.ToDto(theater);
    }
}
