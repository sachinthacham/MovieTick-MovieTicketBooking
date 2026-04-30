using MediatR;
using MovieBooking.Application.DTOs.Showtimes;
using MovieBooking.Application.Features.ShowFormats.Commands;
using MovieBooking.Application.Features.ShowFormats.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.ShowFormats.Handlers;

public class CreateShowFormatHandler : IRequestHandler<CreateShowFormatCommand, ShowFormatDto>
{
    private readonly IShowFormatRepository _repository;

    public CreateShowFormatHandler(IShowFormatRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShowFormatDto> Handle(CreateShowFormatCommand request, CancellationToken cancellationToken)
    {
        var format = new ShowFormat { Name = request.Name, Description = request.Description };
        await _repository.AddAsync(format);
        return new ShowFormatDto { Id = format.Id, Name = format.Name, Description = format.Description };
    }
}

public class UpdateShowFormatHandler : IRequestHandler<UpdateShowFormatCommand, ShowFormatDto>
{
    private readonly IShowFormatRepository _repository;

    public UpdateShowFormatHandler(IShowFormatRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShowFormatDto> Handle(UpdateShowFormatCommand request, CancellationToken cancellationToken)
    {
        var format = await _repository.GetByIdAsync(request.Id);
        if (format == null) throw new KeyNotFoundException("Show format not found.");

        if (!string.IsNullOrWhiteSpace(request.Name)) format.Name = request.Name;
        if (request.Description != null) format.Description = request.Description;
        format.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(format);
        return new ShowFormatDto { Id = format.Id, Name = format.Name, Description = format.Description };
    }
}

public class DeleteShowFormatHandler : IRequestHandler<DeleteShowFormatCommand, bool>
{
    private readonly IShowFormatRepository _repository;

    public DeleteShowFormatHandler(IShowFormatRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteShowFormatCommand request, CancellationToken cancellationToken)
    {
        var format = await _repository.GetByIdAsync(request.Id);
        if (format == null) throw new KeyNotFoundException("Show format not found.");

        await _repository.DeleteAsync(format);
        return true;
    }
}

public class GetAllShowFormatsHandler : IRequestHandler<GetAllShowFormatsQuery, List<ShowFormatDto>>
{
    private readonly IShowFormatRepository _repository;

    public GetAllShowFormatsHandler(IShowFormatRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ShowFormatDto>> Handle(GetAllShowFormatsQuery request, CancellationToken cancellationToken)
    {
        var formats = await _repository.GetAllAsync();
        return formats.Select(f => new ShowFormatDto { Id = f.Id, Name = f.Name, Description = f.Description }).ToList();
    }
}
