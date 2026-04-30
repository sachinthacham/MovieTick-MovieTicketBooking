using MediatR;
using MovieBooking.Application.DTOs.Languages;
using MovieBooking.Application.Features.Languages.Commands;
using MovieBooking.Application.Features.Languages.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Languages.Handlers;

public class CreateLanguageHandler : IRequestHandler<CreateLanguageCommand, LanguageDto>
{
    private readonly ILanguageRepository _languageRepository;

    public CreateLanguageHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<LanguageDto> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = new Language { Name = request.Name, Code = request.Code.ToLower() };
        await _languageRepository.AddAsync(language);
        return new LanguageDto { Id = language.Id, Name = language.Name, Code = language.Code };
    }
}

public class UpdateLanguageHandler : IRequestHandler<UpdateLanguageCommand, LanguageDto>
{
    private readonly ILanguageRepository _languageRepository;

    public UpdateLanguageHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<LanguageDto> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByIdAsync(request.Id);
        if (language == null) throw new KeyNotFoundException("Language not found.");

        if (!string.IsNullOrWhiteSpace(request.Name)) language.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.Code)) language.Code = request.Code.ToLower();
        language.UpdatedAt = DateTime.UtcNow;

        await _languageRepository.UpdateAsync(language);
        return new LanguageDto { Id = language.Id, Name = language.Name, Code = language.Code };
    }
}

public class DeleteLanguageHandler : IRequestHandler<DeleteLanguageCommand, bool>
{
    private readonly ILanguageRepository _languageRepository;

    public DeleteLanguageHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<bool> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByIdAsync(request.Id);
        if (language == null) throw new KeyNotFoundException("Language not found.");

        await _languageRepository.DeleteAsync(language);
        return true;
    }
}

public class GetAllLanguagesHandler : IRequestHandler<GetAllLanguagesQuery, List<LanguageDto>>
{
    private readonly ILanguageRepository _languageRepository;

    public GetAllLanguagesHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<List<LanguageDto>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
    {
        var languages = await _languageRepository.GetAllAsync();
        return languages.Select(l => new LanguageDto { Id = l.Id, Name = l.Name, Code = l.Code }).ToList();
    }
}

public class GetLanguageByIdHandler : IRequestHandler<GetLanguageByIdQuery, LanguageDto>
{
    private readonly ILanguageRepository _languageRepository;

    public GetLanguageByIdHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<LanguageDto> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetByIdAsync(request.Id);
        if (language == null) throw new KeyNotFoundException("Language not found.");

        return new LanguageDto { Id = language.Id, Name = language.Name, Code = language.Code };
    }
}
