using MediatR;
using MovieBooking.Application.DTOs.Languages;

namespace MovieBooking.Application.Features.Languages.Queries;

public record GetAllLanguagesQuery : IRequest<List<LanguageDto>>;
public record GetLanguageByIdQuery(Guid Id) : IRequest<LanguageDto>;
