using MediatR;
using MovieBooking.Application.DTOs.Languages;

namespace MovieBooking.Application.Features.Languages.Commands;

public record CreateLanguageCommand(string Name, string Code) : IRequest<LanguageDto>;
public record UpdateLanguageCommand(Guid Id, string? Name, string? Code) : IRequest<LanguageDto>;
public record DeleteLanguageCommand(Guid Id) : IRequest<bool>;
