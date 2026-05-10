using Microsoft.AspNetCore.Http;

namespace MovieBooking.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(IFormFile file, string folder);
    Task DeleteAsync(string fileUrl);
}
