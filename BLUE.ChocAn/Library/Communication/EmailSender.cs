using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace BLUE.ChocAn.Library.Communication
{
    public class EmailSender : IDisposable
    {
        private string _password;

        #region Constructors

        public EmailSender(string emailHost, int emailPort, string fromAddress, string fromPassword)
        {
            this.EmailHost = emailHost;
            this.EmailPort = emailPort;
            this.FromAddress = fromAddress;
            this._password = fromPassword;
        }

        #endregion

        #region Public Properties

        public string EmailHost { get; private set; }
        public int EmailPort { get; private set; }
        public string FromAddress { get; set; }

        #endregion

        #region Public Methods

        public bool SendEmail(string from, string to, string subject, string message)
        {
            try
            {
                SmtpClient sendSmtpClient = this.GetSmtpClient(this.EmailHost, this.EmailPort, this.FromAddress, this._password);

                using (var mailMessage = new MailMessage(from, to) { Subject = subject, Body = message })
                {
                    Console.WriteLine(string.Format("Sending email to: {0} from: {1}", to, from));
                    sendSmtpClient.Send(mailMessage);
                }

                sendSmtpClient.Dispose();
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
                foreach (MailAddress address in to)
                {
                    this.SendEmail(from, address.Address, subject, message);
                }
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

        private SmtpClient GetSmtpClient(string emailHost, int emailPort, string fromAddress, string password)
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = emailHost,
                Port = emailPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, password)
            };

            return smtp;
        }

        #endregion
    }
}