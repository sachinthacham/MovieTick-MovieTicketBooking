namespace MovieBooking.Application.Interfaces;

public interface IQrCodeService
{
    string GenerateQrCodeBase64(string data);
}
