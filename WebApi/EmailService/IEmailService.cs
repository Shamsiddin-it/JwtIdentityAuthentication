using System;

namespace WebApi.EmailService;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}
