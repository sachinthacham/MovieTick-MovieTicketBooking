using MediatR;
using MovieBooking.Application.DTOs.Genres;
using MovieBooking.Application.Features.Genres.Commands;
using MovieBooking.Application.Features.Genres.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Genres.Handlers;

public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, GenreDto>
{
    private readonly IGenreRepository _genreRepository;

    public CreateGenreHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var existing = await _genreRepository.GetByNameAsync(request.Name);
        if (existing != null)
            throw new InvalidOperationException($"Genre '{request.Name}' already exists.");

        var genre = new Genre { Name = request.Name };
        await _genreRepository.AddAsync(genre);

        return new GenreDto { Id = genre.Id, Name = genre.Name };
    }
}

public class UpdateGenreHandler : IRequestHandler<UpdateGenreCommand, GenreDto>
{
    private readonly IGenreRepository _genreRepository;

    public UpdateGenreHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreDto> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(request.Id);
        if (genre == null) throw new KeyNotFoundException("Genre not found.");

        genre.Name = request.Name;
        genre.UpdatedAt = DateTime.UtcNow;
        await _genreRepository.UpdateAsync(genre);

        return new GenreDto { Id = genre.Id, Name = genre.Name };
    }
}

public class DeleteGenreHandler : IRequestHandler<DeleteGenreCommand, bool>
{
    private readonly IGenreRepository _genreRepository;

    public DeleteGenreHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<bool> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(request.Id);
        if (genre == null) throw new KeyNotFoundException("Genre not found.");

        await _genreRepository.DeleteAsync(genre);
        return true;
    }
}

public class GetAllGenresHandler : IRequestHandler<GetAllGenresQuery, List<GenreDto>>
{
    private readonly IGenreRepository _genreRepository;

    public GetAllGenresHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<List<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = await _genreRepository.GetAllAsync();
        return genres.Select(g => new GenreDto { Id = g.Id, Name = g.Name }).ToList();
    }
}

public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdQuery, GenreDto>
{
    private readonly IGenreRepository _genreRepository;

    public GetGenreByIdHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(request.Id);
        if (genre == null) throw new KeyNotFoundException("Genre not found.");

        return new GenreDto { Id = genre.Id, Name = genre.Name };
    }
}
