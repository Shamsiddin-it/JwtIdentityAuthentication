using System;
using Quartz;
using WebApi.EmailService;

namespace WebApi.Workers;

public class ReportJob(IEmailService emailService) : IJob
{
    private readonly IEmailService _emailService = emailService;
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Generating report...");
        await _emailService.SendAsync("samsiddinarbobov@gmail.com", "testing background jobs", "this is testing process, do not response to email.");
        await Task.CompletedTask;
    }
}
