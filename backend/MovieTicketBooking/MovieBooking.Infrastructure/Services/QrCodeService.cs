using MovieBooking.Application.Interfaces;
using QRCoder;

namespace MovieBooking.Infrastructure.Services;

public class QrCodeService : IQrCodeService
{
    public string GenerateQrCodeBase64(string data)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        var pngBytes = qrCode.GetGraphic(10);
        return Convert.ToBase64String(pngBytes);
    }
}
