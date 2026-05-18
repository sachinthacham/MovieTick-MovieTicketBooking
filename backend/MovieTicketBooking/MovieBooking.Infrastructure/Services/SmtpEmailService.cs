using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;

    public SmtpEmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var host = _config["Smtp:Host"] ?? "smtp.gmail.com";
        var port = _config.GetValue<int>("Smtp:Port", 587);
        var username = _config["Smtp:Username"] ?? string.Empty;
        var password = _config["Smtp:Password"] ?? string.Empty;
        var fromEmail = _config["Smtp:FromEmail"] ?? username;
        var fromName = _config["Smtp:FromName"] ?? "MovieTick";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = body };
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(username, password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendPasswordResetEmailAsync(string to, string resetToken)
    {
        var subject = "Password Reset - MovieTick";
        var body = $"""
            <html>
            <body>
                <h2>Password Reset Request</h2>
                <p>Your password reset token is: <strong>{resetToken}</strong></p>
                <p>This token expires in 1 hour.</p>
                <p>If you did not request a password reset, please ignore this email.</p>
            </body>
            </html>
            """;
        await SendEmailAsync(to, subject, body);
    }
}
