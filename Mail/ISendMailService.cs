using System.Threading.Tasks;

namespace UserManagement.Mail;


public class MailContent
{
  public string To { get; set; }
  public string Subject { get; set; }
  public string Body { get; set; }
}

public interface ISendMailService {
    Task SendMail(MailContent mailContent, CancellationToken ct);

    Task SendEmailAsync(string email, string subject, string htmlMessage);
}