namespace Shop.Web.Helpers
{
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Configuration;
    using MimeKit;

    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration configuration;

        public MailHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMail(string to, string subject, string body)
        {
            var from = this.configuration["Mail:From"];
            var smtp = this.configuration["Mail:Smtp"];
            var port = this.configuration["Mail:Port"];
            var password = this.configuration["Mail:Password"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Administrador Webshop",from));
            message.To.Add(new MailboxAddress("New user",to));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(smtp, int.Parse(port), false);
                client.Authenticate(from, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
