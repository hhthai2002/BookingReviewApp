using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

public class EmailService
{
    private readonly EmailTemplateHelper _emailTemplateHelper;
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration, EmailTemplateHelper emailTemplateHelper)
    {
        _configuration = configuration;
        _emailTemplateHelper = emailTemplateHelper;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("Email:SmtpSettings");

        using var client = new SmtpClient(smtpSettings["Server"], int.Parse(smtpSettings["Port"]))
        {
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        try
        {
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi gửi email: {ex.Message}");
        }
    }

    public async Task<string> GetResetPasswordEmailAsync(string resetLink)
    {
        var emailContent = await _emailTemplateHelper.GetEmailTemplateAsync("reset-password.html");

        if (string.IsNullOrEmpty(emailContent))
            return "Lỗi: Không tìm thấy template email.";

        return emailContent.Replace("{{RESET_LINK}}", resetLink);
    }

    public async Task<string> GetVerificationEmailAsync(string verificationLink)
    {
        var emailContent = await _emailTemplateHelper.GetEmailTemplateAsync("verify-account.html");

        if (string.IsNullOrEmpty(emailContent))
            return "Lỗi: Không tìm thấy template email.";

        return emailContent.Replace("{{VERIFICATION_LINK}}", verificationLink);
    }
}
