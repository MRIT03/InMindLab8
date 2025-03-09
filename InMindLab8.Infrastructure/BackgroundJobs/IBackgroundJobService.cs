namespace InMindLab8.Infrastructure.BackgroundJobs;

public interface IBackgroundJobService
{
    public Task RunHourlyJob();
    public Task SendDailyEmails();
}