using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace UserManagement.Mail;

public class MailSettings
{
  public string From { get; set; }
  public string DisplayName { get; set; }
  public string Username { get; set; }
  public string Password { get; set; }
  public string Host { get; set; }
  public int Port { get; set; }
  public bool UseSSL { get; set; }
  public bool UseStartTls { get; set; }
}

public class SendMailService : ISendMailService
{
  private readonly MailSettings mailSettings;

  private readonly ILogger<SendMailService> _logger;

  public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> logger)
  {
    mailSettings = _mailSettings.Value;
    _logger = logger;
    _logger.LogInformation("Create SendMailService");
  }

  public async Task SendMail(MailContent mailContent, CancellationToken ct = default)
  {
    var email = new MimeMessage();

    email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.From));
    email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.From);
    email.To.Add(MailboxAddress.Parse(mailContent.To));
    email.Subject = mailContent.Subject;

    var builder = new BodyBuilder();
    builder.HtmlBody = mailContent.Body;
    email.Body = builder.ToMessageBody();

    using var smtp = new MailKit.Net.Smtp.SmtpClient();

    try
    {
      if (mailSettings.UseSSL)
      {
        await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.SslOnConnect, ct);
      }
      else
      {
        await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls, ct);
      }
      
      await smtp.AuthenticateAsync(mailSettings.Username, mailSettings.Password, ct);
      await smtp.SendAsync(email, ct);
    }
    catch (Exception ex)
    {
      System.IO.Directory.CreateDirectory("mailssave");
      var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
      await email.WriteToAsync(emailsavefile);

      _logger.LogInformation("Error when send mail. Check file: " + emailsavefile);
      _logger.LogError(ex.Message);
    }

    await smtp.DisconnectAsync(true);
    _logger.LogInformation("Send mail to " + mailContent.To);
  }

  public async Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
    await SendMail(new MailContent()
    {
      To = email,
      Subject = subject,
      Body = htmlMessage
    });
  }
}
