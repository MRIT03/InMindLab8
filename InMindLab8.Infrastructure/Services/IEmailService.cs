namespace InMindLab8.Infrastructure.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}