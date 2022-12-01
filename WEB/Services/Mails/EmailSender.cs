using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace WEB.Services.Mails
{
    public class EmailSender : IEmailSender
    {
        private SmtpClient smtpClient { get; }
        private EmailSenderOptions _options { get; }

        public EmailSender(IOptions<EmailSenderOptions> options)
        {
            _options = options.Value;
            smtpClient = new SmtpClient()
            {
                Host = _options.Host,
                Port = _options.Port,
                //DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_options.Email, _options.Password),
                EnableSsl = _options.EnableSsl,
            };
        }

        public Task SendEmailAsync(string email, string subject, string name, string message, string url)
        {
            ////////
            ///////

            string srcImg = url + "/img/SANITRAZAB.png";
            string img = $"<img width='120' style='width: 120px !important' src='{srcImg}'>";
            //////
            string bodyhtml = "<div style='width: 800px !important; letter-spacing: 1px;margin: 0px;font-family: Helvetica;" +
                " color: #555555 !important;'><div style='font-size: 12px; width: 100%; padding: 25px; background-color: #f2f2f2 !important; border: 2px #e1e1e1 " +
                $"solid;'><div style='width: 120px !important;'>{img}</div><p class='font'><br> " +
                "Estimado/a usuario: <span>" + name + "</span></p><br>" + message +
                "<br><p style='font-size: 9px;'> " +
                "<br> Este mensaje ha sido generado de forma automática. Favor de no responder.</p></div><br><p style='font-size: 9px; " +
                "color: #b2b2b2; text-align: center;'><span>Sistema de Asignación a Proyectos</span> <br><span>PLENUMSOFT</span> <br>" +
                "<span>...</span> <br></p></div>";
            var correo = new MailMessage
            {
                From = new MailAddress(_options.Email),
                To = { email },
                Subject = subject,
                IsBodyHtml = true,
                Body = bodyhtml
            };

            //Se envía el correo
            return smtpClient.SendMailAsync(correo);

        }
    }
}
