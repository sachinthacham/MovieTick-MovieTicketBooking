using MediatR;
using MovieBooking.Application.DTOs.Showtimes;

namespace MovieBooking.Application.Features.ShowFormats.Queries;

public record GetAllShowFormatsQuery : IRequest<List<ShowFormatDto>>;
