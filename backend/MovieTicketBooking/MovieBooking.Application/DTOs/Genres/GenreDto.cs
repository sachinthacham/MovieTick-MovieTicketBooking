namespace MovieBooking.Application.DTOs.Genres;

public class GenreDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class CreateGenreDto
{
    public string Name { get; set; } = default!;
}

public class UpdateGenreDto
{
    public string Name { get; set; } = default!;
}
