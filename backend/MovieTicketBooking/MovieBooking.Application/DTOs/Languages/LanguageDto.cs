namespace MovieBooking.Application.DTOs.Languages;

public class LanguageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
}

public class CreateLanguageDto
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
}

public class UpdateLanguageDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}
