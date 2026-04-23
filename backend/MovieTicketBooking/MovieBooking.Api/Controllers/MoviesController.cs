using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Features.Movies.Queries;
using MovieBooking.Domain.Enums;
using System.Security.Claims;

namespace MovieBooking.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/movies")]
[ApiVersion("1.0")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] Guid? genreId = null,
        [FromQuery] Guid? languageId = null,
        [FromQuery] bool? isFeatured = null,
        [FromQuery] bool? isComingSoon = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? city = null,
        [FromQuery] Guid? formatId = null)
    {
        var result = await _mediator.Send(new GetAllMoviesQuery(page, pageSize, search, genreId, languageId, isFeatured, isComingSoon, isActive, sortBy, city, formatId));
        return Ok(ApiResponse<PagedMoviesDto>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetMovieByIdQuery(id));
        return Ok(ApiResponse<MovieDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovieDto dto)
    {
        var command = new CreateMovieCommand(
            dto.Title, dto.Description, dto.DurationMinutes, dto.ReleaseDate,
            dto.Director, dto.Cast, dto.CertificateRating, dto.IsComingSoon, dto.IsFeatured);
        var movieId = await _mediator.Send(command);
        return Ok(ApiResponse<Guid>.SuccessResponse(movieId, "Movie created."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMovieDto dto)
    {
        var command = new UpdateMovieCommand(id, dto.Title, dto.Description, dto.DurationMinutes, dto.ReleaseDate, dto.Director, dto.Cast, dto.CertificateRating, dto.IsActive);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<MovieDto>.SuccessResponse(result, "Movie updated."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteMovieCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Movie deleted."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/featured")]
    public async Task<IActionResult> ToggleFeatured(Guid id, [FromBody] bool isFeatured)
    {
        await _mediator.Send(new ToggleFeaturedCommand(id, isFeatured));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/coming-soon")]
    public async Task<IActionResult> ToggleComingSoon(Guid id, [FromBody] bool isComingSoon)
    {
        await _mediator.Send(new ToggleComingSoonCommand(id, isComingSoon));
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/genres/{genreId:guid}")]
    public async Task<IActionResult> AddGenre(Guid id, Guid genreId)
    {
        await _mediator.Send(new AddMovieGenreCommand(id, genreId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Genre added."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}/genres/{genreId:guid}")]
    public async Task<IActionResult> RemoveGenre(Guid id, Guid genreId)
    {
        await _mediator.Send(new RemoveMovieGenreCommand(id, genreId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Genre removed."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/languages/{languageId:guid}")]
    public async Task<IActionResult> AddLanguage(Guid id, Guid languageId)
    {
        await _mediator.Send(new AddMovieLanguageCommand(id, languageId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Language added."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}/languages/{languageId:guid}")]
    public async Task<IActionResult> RemoveLanguage(Guid id, Guid languageId)
    {
        await _mediator.Send(new RemoveMovieLanguageCommand(id, languageId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Language removed."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/trailers")]
    public async Task<IActionResult> AddTrailer(Guid id, [FromBody] AddMovieTrailerDto dto)
    {
        var result = await _mediator.Send(new AddMovieTrailerCommand(id, dto.Title, dto.Url, dto.IsPrimary));
        return Ok(ApiResponse<MovieTrailerDto>.SuccessResponse(result, "Trailer added."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("trailers/{trailerId:guid}")]
    public async Task<IActionResult> DeleteTrailer(Guid trailerId)
    {
        await _mediator.Send(new DeleteMovieTrailerCommand(trailerId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Trailer deleted."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/posters")]
    public async Task<IActionResult> UploadPoster(Guid id, IFormFile file, [FromQuery] PosterType posterType = PosterType.Portrait, [FromQuery] bool isPrimary = false)
    {
        var result = await _mediator.Send(new UploadMoviePosterCommand(id, file, posterType, isPrimary));
        return Ok(ApiResponse<MoviePosterDto>.SuccessResponse(result, "Poster uploaded."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("posters/{posterId:guid}")]
    public async Task<IActionResult> DeletePoster(Guid posterId)
    {
        await _mediator.Send(new DeleteMoviePosterCommand(posterId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Poster deleted."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/posters/{posterId:guid}/primary")]
    public async Task<IActionResult> SetPrimaryPoster(Guid id, Guid posterId)
    {
        await _mediator.Send(new SetPrimaryPosterCommand(id, posterId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Primary poster set."));
    }

    [Authorize]
    [HttpPost("{id:guid}/rate")]
    public async Task<IActionResult> Rate(Guid id, [FromBody] RateMovieDto dto)
    {
        var userId = GetCurrentUserId();
        await _mediator.Send(new RateMovieCommand(id, userId, dto.Rating));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Rating submitted."));
    }

    [HttpGet("{id:guid}/reviews")]
    public async Task<IActionResult> GetReviews(Guid id, [FromQuery] bool approvedOnly = true)
    {
        var result = await _mediator.Send(new GetMovieReviewsQuery(id, approvedOnly));
        return Ok(ApiResponse<List<MovieReviewDto>>.SuccessResponse(result));
    }

    [Authorize]
    [HttpPost("{id:guid}/reviews")]
    public async Task<IActionResult> AddReview(Guid id, [FromBody] AddMovieReviewDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new AddMovieReviewCommand(id, userId, dto.ReviewText));
        return Ok(ApiResponse<MovieReviewDto>.SuccessResponse(result, "Review submitted for approval."));
    }

    [Authorize]
    [HttpPut("reviews/{reviewId:guid}")]
    public async Task<IActionResult> UpdateReview(Guid reviewId, [FromBody] UpdateMovieReviewDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new UpdateMovieReviewCommand(reviewId, userId, dto.ReviewText));
        return Ok(ApiResponse<MovieReviewDto>.SuccessResponse(result, "Review updated."));
    }

    [Authorize]
    [HttpDelete("reviews/{reviewId:guid}")]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        var userId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        await _mediator.Send(new DeleteMovieReviewCommand(reviewId, userId, isAdmin));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Review deleted."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("reviews/{reviewId:guid}/approve")]
    public async Task<IActionResult> ApproveReview(Guid reviewId)
    {
        await _mediator.Send(new ApproveMovieReviewCommand(reviewId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Review approved."));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("Invalid token.");
        return userId;
    }
}
