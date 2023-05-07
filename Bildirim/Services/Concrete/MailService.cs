using System.Net.Mail;
using System.Net;
using Bildirim.Models;
using Bildirim.Services.Abstract;

namespace Bildirim.Services.Concrete
{
    public class MailService :IMailService
    {

        private  IConfigurationRoot _config
        {
            get
            {
                return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            }
        }

        public  async Task SendAsync(MailModel model)
        {
            try
            {
                string smtpHost = _config.GetSection("MailingService").GetSection("SmtpHost")?.Value;
                string smtpPort = _config.GetSection("MailingService").GetSection("SmtpPort")?.Value;
                string fromAddress = _config.GetSection("MailingService").GetSection("FromAddress")?.Value;
                string password = _config.GetSection("MailingService").GetSection("Password")?.Value;
                string toAddress = _config.GetSection("MailingService").GetSection("ToAddress")?.Value;




                MailMessage message = new MailMessage();

                message.From = new MailAddress(fromAddress);
                message.To.Add(toAddress);
                message.Body = model.Body;
                message.Subject = model.Subject;
                message.IsBodyHtml = true;


                SmtpClient client = new SmtpClient(smtpHost, int.Parse(smtpPort));

                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(fromAddress, password);

                client.Send(message);


            }
            catch (Exception ex)
            {

            }

        }

    }


}
