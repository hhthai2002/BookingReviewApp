using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

public class EmailTemplateHelper
{
    private readonly IWebHostEnvironment _env;

    public EmailTemplateHelper(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> GetEmailTemplateAsync(string templateFileName)
    {
        var filePath = Path.Combine(_env.WebRootPath, "email-templates", templateFileName);
        if (!File.Exists(filePath))
            return string.Empty;

        return await File.ReadAllTextAsync(filePath);
    }
}
