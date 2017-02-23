using System.Net;
using System.Net.Mail;

namespace TicketManagementSystem.Business
{
    public static class Email
    {
        const string LOGIN = "ticketmanager@mail.ua";
        const string PASSWORD = "";
        const string SMTP_HOST = "smtp.mail.ua";
        const int SMTP_PORT = 587;

        public static bool Send(string email, string subject, string body)
        {
            try
            {
                var client = new SmtpClient
                {
                    Host = SMTP_HOST,
                    Port = SMTP_PORT,
                    Credentials = new NetworkCredential(LOGIN, PASSWORD),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(LOGIN),
                    To = { new MailAddress(email) },
                    Subject = subject,
                    Body = body
                };

                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
