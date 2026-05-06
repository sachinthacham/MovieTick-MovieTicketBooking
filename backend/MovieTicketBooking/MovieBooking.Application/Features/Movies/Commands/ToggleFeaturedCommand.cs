using MediatR;

namespace MovieBooking.Application.Features.Movies.Commands;

public record ToggleFeaturedCommand(Guid MovieId, bool IsFeatured) : IRequest<bool>;
