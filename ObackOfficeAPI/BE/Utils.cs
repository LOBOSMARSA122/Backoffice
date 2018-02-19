using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;

namespace BE
{
    public class Utils
    {
        #region Encription
        public static string Encrypt(string pData)
        {
            UnicodeEncoding parser = new UnicodeEncoding();
            byte[] _original = parser.GetBytes(pData);
            MD5CryptoServiceProvider Hash = new MD5CryptoServiceProvider();
            byte[] _encrypt = Hash.ComputeHash(_original);
            return Convert.ToBase64String(_encrypt);
        }
        #endregion
        #region Mail
        public static bool SendSimpleMail(string body, string subject, List<string> adresses, string SystemAdress, string SystemAdressPassword, string SMTPHost, string MailDisplayName = "Sistema")
        {
            try
            {
                MailMessage Mail = new MailMessage();
                Mail.Body = body;
                Mail.BodyEncoding = Encoding.UTF8;
                Mail.From = new MailAddress(SystemAdress, MailDisplayName);
                Mail.IsBodyHtml = true;
                Mail.Priority = MailPriority.Normal;
                Mail.Subject = subject;
                Mail.To.Add(string.Join(",",adresses));

                SmtpClient Client = new SmtpClient();
                Client.Host = SMTPHost;
                Client.EnableSsl = true;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Port = 587;
                Client.UseDefaultCredentials = false;
                Client.Credentials = new NetworkCredential(SystemAdress, SystemAdressPassword);

                Client.Send(Mail);

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        #endregion
    }
}
