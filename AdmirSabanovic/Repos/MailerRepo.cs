using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace AdmirSabanovic.Repos
{
    public class MailerRepo
    {
        private const String username = "criticalado@gmail.com";
        private const String password = "restart1040z\"";
        public MailerRepo(String to, String subject, String body)
        {
            this.To = to;
            this.Subject = subject;
            this.Body = body;
        }

        public MailerRepo()
        {
            // TODO: Complete member initialization
        }

        public void Send()
        {
            Thread mailer = new Thread(new ThreadStart(sendMail));
            mailer.Start();
        }

        private void sendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("criticalado@gmail.com");
                mail.To.Add(this.To);
                mail.Subject = this.Subject;
                mail.Body = this.Body;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception)
            {
                //Nothing at the moment
            }
        }
        public String To { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
    }
}