namespace MovieBooking.Domain.Entities;

public class MovieLanguage
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = default!;

    public Guid LanguageId { get; set; }
    public Language Language { get; set; } = default!;
}
