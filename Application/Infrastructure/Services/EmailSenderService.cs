using Application.Common.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Application.Infrastructure.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public EmailSenderService(
        ILogger<EmailSenderService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    //public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.
    /*
      if (!sendGridKey["SendGridKey"].ToString())
      {
          throw new Exception("Null SendGridKey");
    }*/

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var config = _configuration.GetSection("Authentication").GetSection("Email");
        await Execute(config["SendGridKey"], subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var config = _configuration.GetSection("Authentication").GetSection("Email");

        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(config["EmailAddress"], config["EmailAddressName"]),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode 
            ? $"Email to {toEmail} queued successfully!"
            : $"Failure Email to {toEmail}");
    }  
}