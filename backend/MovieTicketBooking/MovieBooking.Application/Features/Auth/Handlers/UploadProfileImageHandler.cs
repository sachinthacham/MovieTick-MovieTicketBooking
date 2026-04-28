using MediatR;
using MovieBooking.Application.Features.Auth.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Auth.Handlers;

public class UploadProfileImageHandler : IRequestHandler<UploadProfileImageCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IFileStorageService _fileStorage;

    public UploadProfileImageHandler(IUserRepository userRepository, IFileStorageService fileStorage)
    {
        _userRepository = userRepository;
        _fileStorage = fileStorage;
    }

    public async Task<string> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            await _fileStorage.DeleteAsync(user.ProfileImageUrl);

        var url = await _fileStorage.UploadAsync(request.File, "profiles");

        user.ProfileImageUrl = url;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return url;
    }
}
