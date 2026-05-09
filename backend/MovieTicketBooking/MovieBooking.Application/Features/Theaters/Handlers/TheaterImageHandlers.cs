using MediatR;
using MovieBooking.Application.DTOs.Theaters;
using MovieBooking.Application.Features.Theaters.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Theaters.Handlers;

public class UploadTheaterImageHandler : IRequestHandler<UploadTheaterImageCommand, TheaterImageDto>
{
    private readonly ITheaterImageRepository _imageRepository;
    private readonly ITheaterRepository _theaterRepository;
    private readonly IFileStorageService _fileStorage;

    public UploadTheaterImageHandler(
        ITheaterImageRepository imageRepository,
        ITheaterRepository theaterRepository,
        IFileStorageService fileStorage)
    {
        _imageRepository = imageRepository;
        _theaterRepository = theaterRepository;
        _fileStorage = fileStorage;
    }

    public async Task<TheaterImageDto> Handle(UploadTheaterImageCommand request, CancellationToken cancellationToken)
    {
        var theater = await _theaterRepository.GetByIdAsync(request.TheaterId);
        if (theater == null) throw new KeyNotFoundException("Theater not found.");

        if (request.IsPrimary)
            await _imageRepository.ClearPrimaryAsync(request.TheaterId);

        var url = await _fileStorage.UploadAsync(request.File, "theater-images");

        var image = new TheaterImage
        {
            TheaterId = request.TheaterId,
            ImageUrl = url,
            IsPrimary = request.IsPrimary
        };

        await _imageRepository.AddAsync(image);

        return new TheaterImageDto { Id = image.Id, ImageUrl = image.ImageUrl, IsPrimary = image.IsPrimary };
    }
}

public class DeleteTheaterImageHandler : IRequestHandler<DeleteTheaterImageCommand, bool>
{
    private readonly ITheaterImageRepository _imageRepository;
    private readonly IFileStorageService _fileStorage;

    public DeleteTheaterImageHandler(ITheaterImageRepository imageRepository, IFileStorageService fileStorage)
    {
        _imageRepository = imageRepository;
        _fileStorage = fileStorage;
    }

    public async Task<bool> Handle(DeleteTheaterImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.GetByIdAsync(request.ImageId);
        if (image == null) throw new KeyNotFoundException("Image not found.");

        await _fileStorage.DeleteAsync(image.ImageUrl);
        await _imageRepository.DeleteAsync(image);
        return true;
    }
}

public class SetPrimaryTheaterImageHandler : IRequestHandler<SetPrimaryTheaterImageCommand, bool>
{
    private readonly ITheaterImageRepository _imageRepository;

    public SetPrimaryTheaterImageHandler(ITheaterImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<bool> Handle(SetPrimaryTheaterImageCommand request, CancellationToken cancellationToken)
    {
        await _imageRepository.ClearPrimaryAsync(request.TheaterId);

        var image = await _imageRepository.GetByIdAsync(request.ImageId);
        if (image == null) throw new KeyNotFoundException("Image not found.");

        image.IsPrimary = true;
        await _imageRepository.UpdateAsync(image);
        return true;
    }
}
