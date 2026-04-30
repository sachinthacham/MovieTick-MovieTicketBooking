using MediatR;
using MovieBooking.Application.DTOs.Showtimes;
using MovieBooking.Application.Features.Showtimes.Commands;
using MovieBooking.Application.Features.Showtimes.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Features.Showtimes.Handlers;

internal static class ShowtimeMapper
{
    internal static ShowtimeDto ToDto(Showtime s) => new()
    {
        Id = s.Id,
        MovieId = s.MovieId,
        MovieTitle = s.Movie?.Title ?? string.Empty,
        ScreenId = s.ScreenId,
        ScreenName = s.Screen?.Name ?? string.Empty,
        TheaterName = s.Screen?.Theater?.Name ?? string.Empty,
        ShowFormatName = s.ShowFormat?.Name ?? string.Empty,
        LanguageName = s.Language?.Name ?? string.Empty,
        StartTime = s.StartTime,
        EndTime = s.EndTime,
        Status = s.Status.ToString(),
        CreatedAt = s.CreatedAt,
        Pricings = s.Pricings.Select(p => new ShowtimePricingDto
        {
            Id = p.Id,
            SeatCategoryId = p.SeatCategoryId,
            SeatCategoryName = p.SeatCategory?.Name ?? string.Empty,
            Price = p.Price
        }).ToList()
    };
}

public class CreateShowtimeHandler : IRequestHandler<CreateShowtimeCommand, ShowtimeDto>
{
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IScreenRepository _screenRepository;
    private readonly IShowFormatRepository _showFormatRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IShowtimeSeatRepository _showtimeSeatRepository;

    public CreateShowtimeHandler(
        IShowtimeRepository showtimeRepository,
        IMovieRepository movieRepository,
        IScreenRepository screenRepository,
        IShowFormatRepository showFormatRepository,
        ILanguageRepository languageRepository,
        ISeatRepository seatRepository,
        IShowtimeSeatRepository showtimeSeatRepository)
    {
        _showtimeRepository = showtimeRepository;
        _movieRepository = movieRepository;
        _screenRepository = screenRepository;
        _showFormatRepository = showFormatRepository;
        _languageRepository = languageRepository;
        _seatRepository = seatRepository;
        _showtimeSeatRepository = showtimeSeatRepository;
    }

    public async Task<ShowtimeDto> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        var screen = await _screenRepository.GetByIdAsync(request.ScreenId);
        if (screen == null) throw new KeyNotFoundException("Screen not found.");

        var format = await _showFormatRepository.GetByIdAsync(request.ShowFormatId);
        if (format == null) throw new KeyNotFoundException("Show format not found.");

        var language = await _languageRepository.GetByIdAsync(request.LanguageId);
        if (language == null) throw new KeyNotFoundException("Language not found.");

        var endTime = request.StartTime.AddMinutes(movie.DurationMinutes);

        var showtime = new Showtime
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            ShowFormatId = request.ShowFormatId,
            LanguageId = request.LanguageId,
            StartTime = request.StartTime,
            EndTime = endTime,
            Movie = movie,
            Screen = screen,
            ShowFormat = format,
            Language = language
        };

        await _showtimeRepository.AddAsync(showtime);

        var activeSeats = await _seatRepository.GetActiveByScreenAsync(request.ScreenId);
        var showtimeSeats = activeSeats.Select(seat => new ShowtimeSeat
        {
            ShowtimeId = showtime.Id,
            SeatId = seat.Id,
            Status = SeatStatus.Available
        }).ToList();

        await _showtimeSeatRepository.AddRangeAsync(showtimeSeats);

        return ShowtimeMapper.ToDto(showtime);
    }
}

public class UpdateShowtimeHandler : IRequestHandler<UpdateShowtimeCommand, ShowtimeDto>
{
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly IShowFormatRepository _showFormatRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IMovieRepository _movieRepository;

    public UpdateShowtimeHandler(
        IShowtimeRepository showtimeRepository,
        IShowFormatRepository showFormatRepository,
        ILanguageRepository languageRepository,
        IMovieRepository movieRepository)
    {
        _showtimeRepository = showtimeRepository;
        _showFormatRepository = showFormatRepository;
        _languageRepository = languageRepository;
        _movieRepository = movieRepository;
    }

    public async Task<ShowtimeDto> Handle(UpdateShowtimeCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdAsync(request.Id);
        if (showtime == null) throw new KeyNotFoundException("Showtime not found.");

        if (request.ShowFormatId.HasValue)
        {
            var format = await _showFormatRepository.GetByIdAsync(request.ShowFormatId.Value);
            if (format == null) throw new KeyNotFoundException("Show format not found.");
            showtime.ShowFormatId = request.ShowFormatId.Value;
            showtime.ShowFormat = format;
        }

        if (request.LanguageId.HasValue)
        {
            var language = await _languageRepository.GetByIdAsync(request.LanguageId.Value);
            if (language == null) throw new KeyNotFoundException("Language not found.");
            showtime.LanguageId = request.LanguageId.Value;
            showtime.Language = language;
        }

        if (request.StartTime.HasValue)
        {
            var movie = await _movieRepository.GetByIdAsync(showtime.MovieId);
            showtime.StartTime = request.StartTime.Value;
            showtime.EndTime = request.StartTime.Value.AddMinutes(movie?.DurationMinutes ?? 0);
        }

        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<ShowtimeStatus>(request.Status, out var status))
            showtime.Status = status;

        showtime.UpdatedAt = DateTime.UtcNow;
        await _showtimeRepository.UpdateAsync(showtime);
        return ShowtimeMapper.ToDto(showtime);
    }
}

public class DeleteShowtimeHandler : IRequestHandler<DeleteShowtimeCommand, bool>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public DeleteShowtimeHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public async Task<bool> Handle(DeleteShowtimeCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdAsync(request.Id);
        if (showtime == null) throw new KeyNotFoundException("Showtime not found.");

        showtime.Status = ShowtimeStatus.Cancelled;
        showtime.UpdatedAt = DateTime.UtcNow;
        await _showtimeRepository.UpdateAsync(showtime);
        return true;
    }
}

public class SetShowtimePricingHandler : IRequestHandler<SetShowtimePricingCommand, ShowtimePricingDto>
{
    private readonly IShowtimePricingRepository _pricingRepository;
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly ISeatCategoryRepository _categoryRepository;

    public SetShowtimePricingHandler(
        IShowtimePricingRepository pricingRepository,
        IShowtimeRepository showtimeRepository,
        ISeatCategoryRepository categoryRepository)
    {
        _pricingRepository = pricingRepository;
        _showtimeRepository = showtimeRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ShowtimePricingDto> Handle(SetShowtimePricingCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdAsync(request.ShowtimeId);
        if (showtime == null) throw new KeyNotFoundException("Showtime not found.");

        var category = await _categoryRepository.GetByIdAsync(request.SeatCategoryId);
        if (category == null) throw new KeyNotFoundException("Seat category not found.");

        var existing = await _pricingRepository.GetByShowtimeAndCategoryAsync(request.ShowtimeId, request.SeatCategoryId);
        if (existing != null)
        {
            existing.Price = request.Price;
            existing.UpdatedAt = DateTime.UtcNow;
            await _pricingRepository.UpdateAsync(existing);
            return new ShowtimePricingDto
            {
                Id = existing.Id,
                SeatCategoryId = existing.SeatCategoryId,
                SeatCategoryName = category.Name,
                Price = existing.Price
            };
        }

        var pricing = new ShowtimePricing
        {
            ShowtimeId = request.ShowtimeId,
            SeatCategoryId = request.SeatCategoryId,
            Price = request.Price,
            SeatCategory = category
        };

        await _pricingRepository.AddAsync(pricing);

        return new ShowtimePricingDto
        {
            Id = pricing.Id,
            SeatCategoryId = pricing.SeatCategoryId,
            SeatCategoryName = category.Name,
            Price = pricing.Price
        };
    }
}

public class GetShowtimeByIdHandler : IRequestHandler<GetShowtimeByIdQuery, ShowtimeDto>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public GetShowtimeByIdHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public async Task<ShowtimeDto> Handle(GetShowtimeByIdQuery request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdAsync(request.Id);
        if (showtime == null) throw new KeyNotFoundException("Showtime not found.");
        return ShowtimeMapper.ToDto(showtime);
    }
}

public class GetShowtimesByMovieHandler : IRequestHandler<GetShowtimesByMovieQuery, List<ShowtimeDto>>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public GetShowtimesByMovieHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public async Task<List<ShowtimeDto>> Handle(GetShowtimesByMovieQuery request, CancellationToken cancellationToken)
    {
        var showtimes = await _showtimeRepository.GetByMovieAsync(request.MovieId, request.Date);
        return showtimes.Select(ShowtimeMapper.ToDto).ToList();
    }
}

public class GetShowtimesByTheaterHandler : IRequestHandler<GetShowtimesByTheaterQuery, List<ShowtimeDto>>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public GetShowtimesByTheaterHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public async Task<List<ShowtimeDto>> Handle(GetShowtimesByTheaterQuery request, CancellationToken cancellationToken)
    {
        var showtimes = await _showtimeRepository.GetByTheaterAsync(request.TheaterId, request.Date);
        return showtimes.Select(ShowtimeMapper.ToDto).ToList();
    }
}

public class GetShowtimesByScreenHandler : IRequestHandler<GetShowtimesByScreenQuery, List<ShowtimeDto>>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public GetShowtimesByScreenHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public async Task<List<ShowtimeDto>> Handle(GetShowtimesByScreenQuery request, CancellationToken cancellationToken)
    {
        var showtimes = await _showtimeRepository.GetByScreenAsync(request.ScreenId, request.Date);
        return showtimes.Select(ShowtimeMapper.ToDto).ToList();
    }
}

public class GetRecentShowtimesHandler : IRequestHandler<GetRecentShowtimesQuery, List<ShowtimeDto>>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public GetRecentShowtimesHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public async Task<List<ShowtimeDto>> Handle(GetRecentShowtimesQuery request, CancellationToken cancellationToken)
    {
        var showtimes = await _showtimeRepository.GetRecentAsync(request.Take);
        return showtimes.Select(ShowtimeMapper.ToDto).ToList();
    }
}

public class GetShowtimeCountHandler : IRequestHandler<GetShowtimeCountQuery, int>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public GetShowtimeCountHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public Task<int> Handle(GetShowtimeCountQuery request, CancellationToken cancellationToken)
        => _showtimeRepository.CountAsync(cancellationToken);
}
