using MediatR;
using MovieBooking.Application.DTOs.Screens;
using MovieBooking.Application.Features.Screens.Commands;
using MovieBooking.Application.Features.Screens.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Screens.Handlers;

internal static class ScreenMappingHelper
{
    internal static ScreenDto ToDto(Screen screen) => new()
    {
        Id = screen.Id,
        TheaterId = screen.TheaterId,
        Name = screen.Name,
        TotalSeats = screen.TotalSeats,
        ScreenType = screen.ScreenType,
        IsActive = screen.IsActive,
        CreatedAt = screen.CreatedAt
    };
}

public class CreateScreenHandler : IRequestHandler<CreateScreenCommand, ScreenDto>
{
    private readonly IScreenRepository _screenRepository;
    private readonly ITheaterRepository _theaterRepository;

    public CreateScreenHandler(IScreenRepository screenRepository, ITheaterRepository theaterRepository)
    {
        _screenRepository = screenRepository;
        _theaterRepository = theaterRepository;
    }

    public async Task<ScreenDto> Handle(CreateScreenCommand request, CancellationToken cancellationToken)
    {
        var theater = await _theaterRepository.GetByIdAsync(request.TheaterId);
        if (theater == null) throw new KeyNotFoundException("Theater not found.");

        var screen = new Screen
        {
            TheaterId = request.TheaterId,
            Name = request.Name,
            ScreenType = request.ScreenType
        };

        await _screenRepository.AddAsync(screen);
        return ScreenMappingHelper.ToDto(screen);
    }
}

public class UpdateScreenHandler : IRequestHandler<UpdateScreenCommand, ScreenDto>
{
    private readonly IScreenRepository _screenRepository;

    public UpdateScreenHandler(IScreenRepository screenRepository)
    {
        _screenRepository = screenRepository;
    }

    public async Task<ScreenDto> Handle(UpdateScreenCommand request, CancellationToken cancellationToken)
    {
        var screen = await _screenRepository.GetByIdAsync(request.Id);
        if (screen == null) throw new KeyNotFoundException("Screen not found.");

        if (!string.IsNullOrWhiteSpace(request.Name)) screen.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.ScreenType)) screen.ScreenType = request.ScreenType;
        if (request.IsActive.HasValue) screen.IsActive = request.IsActive.Value;
        screen.UpdatedAt = DateTime.UtcNow;

        await _screenRepository.UpdateAsync(screen);
        return ScreenMappingHelper.ToDto(screen);
    }
}

public class DeleteScreenHandler : IRequestHandler<DeleteScreenCommand, bool>
{
    private readonly IScreenRepository _screenRepository;

    public DeleteScreenHandler(IScreenRepository screenRepository)
    {
        _screenRepository = screenRepository;
    }

    public async Task<bool> Handle(DeleteScreenCommand request, CancellationToken cancellationToken)
    {
        var screen = await _screenRepository.GetByIdAsync(request.Id);
        if (screen == null) throw new KeyNotFoundException("Screen not found.");

        await _screenRepository.DeleteAsync(screen);
        return true;
    }
}

public class GetScreensByTheaterHandler : IRequestHandler<GetScreensByTheaterQuery, List<ScreenDto>>
{
    private readonly IScreenRepository _screenRepository;

    public GetScreensByTheaterHandler(IScreenRepository screenRepository)
    {
        _screenRepository = screenRepository;
    }

    public async Task<List<ScreenDto>> Handle(GetScreensByTheaterQuery request, CancellationToken cancellationToken)
    {
        var screens = await _screenRepository.GetByTheaterAsync(request.TheaterId);
        return screens.Select(ScreenMappingHelper.ToDto).ToList();
    }
}

public class GetScreenByIdHandler : IRequestHandler<GetScreenByIdQuery, ScreenDto>
{
    private readonly IScreenRepository _screenRepository;

    public GetScreenByIdHandler(IScreenRepository screenRepository)
    {
        _screenRepository = screenRepository;
    }

    public async Task<ScreenDto> Handle(GetScreenByIdQuery request, CancellationToken cancellationToken)
    {
        var screen = await _screenRepository.GetByIdAsync(request.Id);
        if (screen == null) throw new KeyNotFoundException("Screen not found.");
        return ScreenMappingHelper.ToDto(screen);
    }
}
