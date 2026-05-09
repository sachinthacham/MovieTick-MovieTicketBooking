using MediatR;
using MovieBooking.Application.Features.Theaters.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Theaters.Handlers;

public class GetDistinctCitiesHandler : IRequestHandler<GetDistinctCitiesQuery, List<string>>
{
    private readonly ITheaterRepository _theaterRepository;

    public GetDistinctCitiesHandler(ITheaterRepository theaterRepository)
    {
        _theaterRepository = theaterRepository;
    }

    public async Task<List<string>> Handle(GetDistinctCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _theaterRepository.GetDistinctCitiesAsync();
    }
}
