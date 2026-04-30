using MediatR;
using MovieBooking.Application.DTOs.Seats;
using MovieBooking.Application.Features.SeatCategories.Commands;
using MovieBooking.Application.Features.SeatCategories.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.SeatCategories.Handlers;

internal static class SeatCategoryMapper
{
    internal static SeatCategoryDto ToDto(SeatCategory c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        DefaultPrice = c.DefaultPrice,
        Color = c.Color,
        Description = c.Description
    };
}

public class CreateSeatCategoryHandler : IRequestHandler<CreateSeatCategoryCommand, SeatCategoryDto>
{
    private readonly ISeatCategoryRepository _categoryRepository;

    public CreateSeatCategoryHandler(ISeatCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<SeatCategoryDto> Handle(CreateSeatCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new SeatCategory
        {
            Name = request.Name,
            DefaultPrice = request.DefaultPrice,
            Color = request.Color,
            Description = request.Description
        };

        await _categoryRepository.AddAsync(category);
        return SeatCategoryMapper.ToDto(category);
    }
}

public class UpdateSeatCategoryHandler : IRequestHandler<UpdateSeatCategoryCommand, SeatCategoryDto>
{
    private readonly ISeatCategoryRepository _categoryRepository;

    public UpdateSeatCategoryHandler(ISeatCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<SeatCategoryDto> Handle(UpdateSeatCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null) throw new KeyNotFoundException("Seat category not found.");

        if (!string.IsNullOrWhiteSpace(request.Name)) category.Name = request.Name;
        if (request.DefaultPrice.HasValue) category.DefaultPrice = request.DefaultPrice.Value;
        if (!string.IsNullOrWhiteSpace(request.Color)) category.Color = request.Color;
        if (request.Description != null) category.Description = request.Description;
        category.UpdatedAt = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(category);
        return SeatCategoryMapper.ToDto(category);
    }
}

public class DeleteSeatCategoryHandler : IRequestHandler<DeleteSeatCategoryCommand, bool>
{
    private readonly ISeatCategoryRepository _categoryRepository;

    public DeleteSeatCategoryHandler(ISeatCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteSeatCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null) throw new KeyNotFoundException("Seat category not found.");

        await _categoryRepository.DeleteAsync(category);
        return true;
    }
}

public class GetAllSeatCategoriesHandler : IRequestHandler<GetAllSeatCategoriesQuery, List<SeatCategoryDto>>
{
    private readonly ISeatCategoryRepository _categoryRepository;

    public GetAllSeatCategoriesHandler(ISeatCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<SeatCategoryDto>> Handle(GetAllSeatCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(SeatCategoryMapper.ToDto).ToList();
    }
}

public class GetSeatCategoryByIdHandler : IRequestHandler<GetSeatCategoryByIdQuery, SeatCategoryDto>
{
    private readonly ISeatCategoryRepository _categoryRepository;

    public GetSeatCategoryByIdHandler(ISeatCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<SeatCategoryDto> Handle(GetSeatCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null) throw new KeyNotFoundException("Seat category not found.");
        return SeatCategoryMapper.ToDto(category);
    }
}
