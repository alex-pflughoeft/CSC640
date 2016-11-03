using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace BLUE.ChocAn.Library.Communication
{
    public class EmailSender : IDisposable
    {
        #region Public Properties

        public string EmailHost { get; private set; }
        public int EmailPort { get; private set; }

        #endregion

        #region Constructors

        public EmailSender(string emailHost, int emailPort)
        {
            this.EmailHost = emailHost;
            this.EmailPort = emailPort;
        }

        #endregion

        #region Public Methods

        public bool SendEmail(string from, string to, string subject, string message)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(from, to, subject, message);
                Console.WriteLine(string.Format("Sending email to: {0} from: {1}", to, from));

                SmtpClient sendSmtpClient = this.GetSmtpClient(this.EmailHost, this.EmailPort);

                sendSmtpClient.Send(mailMessage);

                sendSmtpClient.Dispose();
                mailMessage.Dispose();            
            }
            catch (Exception e)
            {
                Console.WriteLine("Failure sending email from: {0} to: {1} on email host: {2}", from, to, this.EmailHost);
                return false;
            }

            return true;
        }

        public bool SendToMultiple(string from, List<MailAddress> to, string subject, string message)
        {
            try
            {
                // TODO: Send the email to multiple
            }
            catch (Exception e)
            {
                Console.WriteLine("Failure sending email from: {0} to: {1} on email host: {2}", from, to, this.EmailHost);
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private SmtpClient GetSmtpClient(string emailHost, int emailPort)
        {
            return new SmtpClient(emailHost, emailPort);
        }

        #endregion
    }
}