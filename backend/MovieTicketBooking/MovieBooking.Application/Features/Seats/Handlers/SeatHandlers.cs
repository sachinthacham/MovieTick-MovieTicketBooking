using MediatR;
using MovieBooking.Application.DTOs.Seats;
using MovieBooking.Application.Features.Seats.Commands;
using MovieBooking.Application.Features.Seats.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Seats.Handlers;

internal static class SeatMapper
{
    internal static SeatDto ToDto(Seat seat) => new()
    {
        Id = seat.Id,
        ScreenId = seat.ScreenId,
        SeatCategoryId = seat.SeatCategoryId,
        SeatCategoryName = seat.SeatCategory?.Name ?? string.Empty,
        SeatNumber = seat.SeatNumber,
        Row = seat.Row,
        Column = seat.Column,
        IsBlocked = seat.IsBlocked,
        IsActive = seat.IsActive
    };
}

public class BulkCreateSeatsHandler : IRequestHandler<BulkCreateSeatsCommand, List<SeatDto>>
{
    private readonly ISeatRepository _seatRepository;
    private readonly IScreenRepository _screenRepository;
    private readonly ISeatCategoryRepository _categoryRepository;

    public BulkCreateSeatsHandler(ISeatRepository seatRepository, IScreenRepository screenRepository, ISeatCategoryRepository categoryRepository)
    {
        _seatRepository = seatRepository;
        _screenRepository = screenRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<SeatDto>> Handle(BulkCreateSeatsCommand request, CancellationToken cancellationToken)
    {
        var screen = await _screenRepository.GetByIdAsync(request.ScreenId);
        if (screen == null) throw new KeyNotFoundException("Screen not found.");

        var seats = new List<Seat>();
        foreach (var rowConfig in request.Rows)
        {
            var category = await _categoryRepository.GetByIdAsync(rowConfig.SeatCategoryId);
            if (category == null) throw new KeyNotFoundException($"Seat category {rowConfig.SeatCategoryId} not found.");

            for (int col = 1; col <= rowConfig.NumberOfSeats; col++)
            {
                seats.Add(new Seat
                {
                    ScreenId = request.ScreenId,
                    SeatCategoryId = rowConfig.SeatCategoryId,
                    Row = rowConfig.RowLabel,
                    Column = col,
                    SeatNumber = $"{rowConfig.RowLabel}{col}",
                    SeatCategory = category
                });
            }
        }

        await _seatRepository.AddRangeAsync(seats);

        screen.TotalSeats = (await _seatRepository.GetByScreenAsync(request.ScreenId)).Count;
        await _screenRepository.UpdateAsync(screen);

        return seats.Select(SeatMapper.ToDto).ToList();
    }
}

public class UpdateSeatHandler : IRequestHandler<UpdateSeatCommand, SeatDto>
{
    private readonly ISeatRepository _seatRepository;
    private readonly ISeatCategoryRepository _categoryRepository;

    public UpdateSeatHandler(ISeatRepository seatRepository, ISeatCategoryRepository categoryRepository)
    {
        _seatRepository = seatRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<SeatDto> Handle(UpdateSeatCommand request, CancellationToken cancellationToken)
    {
        var seat = await _seatRepository.GetByIdAsync(request.SeatId);
        if (seat == null) throw new KeyNotFoundException("Seat not found.");

        if (request.SeatCategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(request.SeatCategoryId.Value);
            if (category == null) throw new KeyNotFoundException("Seat category not found.");
            seat.SeatCategoryId = request.SeatCategoryId.Value;
            seat.SeatCategory = category;
        }

        if (request.IsActive.HasValue) seat.IsActive = request.IsActive.Value;
        seat.UpdatedAt = DateTime.UtcNow;

        await _seatRepository.UpdateAsync(seat);
        return SeatMapper.ToDto(seat);
    }
}

public class BlockSeatHandler : IRequestHandler<BlockSeatCommand, bool>
{
    private readonly ISeatRepository _seatRepository;

    public BlockSeatHandler(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public async Task<bool> Handle(BlockSeatCommand request, CancellationToken cancellationToken)
    {
        var seat = await _seatRepository.GetByIdAsync(request.SeatId);
        if (seat == null) throw new KeyNotFoundException("Seat not found.");

        seat.IsBlocked = true;
        seat.UpdatedAt = DateTime.UtcNow;
        await _seatRepository.UpdateAsync(seat);
        return true;
    }
}

public class UnblockSeatHandler : IRequestHandler<UnblockSeatCommand, bool>
{
    private readonly ISeatRepository _seatRepository;

    public UnblockSeatHandler(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public async Task<bool> Handle(UnblockSeatCommand request, CancellationToken cancellationToken)
    {
        var seat = await _seatRepository.GetByIdAsync(request.SeatId);
        if (seat == null) throw new KeyNotFoundException("Seat not found.");

        seat.IsBlocked = false;
        seat.UpdatedAt = DateTime.UtcNow;
        await _seatRepository.UpdateAsync(seat);
        return true;
    }
}

public class GetSeatsByScreenHandler : IRequestHandler<GetSeatsByScreenQuery, List<SeatDto>>
{
    private readonly ISeatRepository _seatRepository;

    public GetSeatsByScreenHandler(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public async Task<List<SeatDto>> Handle(GetSeatsByScreenQuery request, CancellationToken cancellationToken)
    {
        var seats = await _seatRepository.GetByScreenAsync(request.ScreenId);
        return seats.Select(SeatMapper.ToDto).ToList();
    }
}

public class GetSeatAvailabilityHandler : IRequestHandler<GetSeatAvailabilityQuery, List<SeatAvailabilityDto>>
{
    private readonly IShowtimeSeatRepository _showtimeSeatRepository;
    private readonly IShowtimePricingRepository _pricingRepository;

    public GetSeatAvailabilityHandler(IShowtimeSeatRepository showtimeSeatRepository, IShowtimePricingRepository pricingRepository)
    {
        _showtimeSeatRepository = showtimeSeatRepository;
        _pricingRepository = pricingRepository;
    }

    public async Task<List<SeatAvailabilityDto>> Handle(GetSeatAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var showtimeSeats = await _showtimeSeatRepository.GetByShowtimeAsync(request.ShowtimeId);
        var pricings = await _pricingRepository.GetByShowtimeAsync(request.ShowtimeId);

        var result = new List<SeatAvailabilityDto>();
        foreach (var ss in showtimeSeats)
        {
            var pricing = pricings.FirstOrDefault(p => p.SeatCategoryId == ss.Seat.SeatCategoryId);
            result.Add(new SeatAvailabilityDto
            {
                Id = ss.Id,
                SeatId = ss.SeatId,
                SeatCategoryId = ss.Seat.SeatCategoryId,
                SeatNumber = ss.Seat.SeatNumber,
                Row = ss.Seat.Row,
                Column = ss.Seat.Column,
                SeatCategoryName = ss.Seat.SeatCategory?.Name ?? string.Empty,
                Color = ss.Seat.SeatCategory?.Color ?? "#000",
                Status = ss.Status.ToString(),
                Price = pricing?.Price
            });
        }

        return result.OrderBy(s => s.Row).ThenBy(s => s.Column).ToList();
    }
}
