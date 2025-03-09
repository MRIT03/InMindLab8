using InMindLab8.Application.Commands;
using InMindLab8.Application.Queries;
using MailKit.Net.Smtp;
using MediatR;
using MimeKit;
using InMindLab8.Infrastructure.Services;

namespace InMindLab8.Infrastructure.BackgroundJobs;

public class BackgroundJobService : IBackgroundJobService
{
    private readonly ILogger<BackgroundJobService> _logger;
    private readonly IMediator _mediator;
    private readonly IEmailService _emailService;

    public BackgroundJobService(ILogger<BackgroundJobService> logger, IMediator mediator, IEmailService emailService)
    {
        _logger = logger;
        _mediator = mediator;
        _emailService = emailService;
    }

    public async Task RunHourlyJob()
    {
        _logger.LogInformation("Hourly job executed at: {Time}", DateTime.UtcNow);
        // Business logic for hourly job
        var result = await _mediator.Send(new UpdateGPACommand());
        _logger.LogInformation("Finished executing the GPA calculator task with the following result: {result}",
            result.IsSuccess ? result.Value : result.Error);
        _logger.LogInformation("Hourly job finished executing at: {Time}", DateTime.UtcNow);
    }


    public async Task SendDailyEmails()
    {
        var mailingList = await _mediator.Send(new CreateMailingListQuery());
        _logger.LogInformation("Daily mailing job executed at: {Time}", DateTime.UtcNow);
        foreach (var entry in mailingList)
        {
            var to = entry.Key.Email;
            var subject = "Reminder: " + entry.Value.Name + " Deadline";
            var Body = "The deadline for enrollment for the following course:" + entry.Value.Name +
                       "\nIs on the following day:" + entry.Value.EnrollEnd;
            _emailService.SendEmailAsync(to, subject, Body);
        }

        Console.WriteLine("Email sent successfully (Mailtrap)!");
    }
}