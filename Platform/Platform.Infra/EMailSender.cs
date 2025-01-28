using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infra
{
    public class EMailSender
    {
        /// <summary> 寄送信件 </summary>
        /// <param name="receiverMailList"> 收件人 </param>
        /// <param name="content"> 內容 </param>
        public static void Send(List<MailAddress> receiverMailList, EMailContent content)
        {
            var config = EmailConfig.GetDefault();

            if (config == null)
                throw new NullReferenceException("Email Config is imcompleted. ");

            if (!config.WillSendMail)
                return;


            SmtpClient client = new SmtpClient();
            client.Host = config.SmtpHost;
            client.Port = config.SmtpPort;
            client.Credentials = new NetworkCredential(config.SmtpAccount, config.SmtpPassword);
            client.EnableSsl = true;

            try
            {
                foreach(var item in  receiverMailList)
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(config.SmtpAccount);
                    mail.To.Add(item);
                    mail.Subject = content.Title;
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Body = content.Body;
                    mail.BodyEncoding = Encoding.UTF8;

                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Dispose();
            }
        }

        /// <summary> 寄送信件 </summary>
        /// <param name="receiverMailList"> 收件人 </param>
        /// <param name="ccList"> CC 收件人 </param>
        /// <param name="content"> 內容 </param>
        public static void SendWithCC(List<MailAddress> receiverMailList, List<MailAddress> ccList, EMailContent content)
        {
            var config = EmailConfig.GetDefault();

            if (config == null)
                throw new NullReferenceException("Email Config is imcompleted. ");

            if (!config.WillSendMail)
                return;


            SmtpClient client = new SmtpClient();
            client.Host = config.SmtpHost;
            client.Port = config.SmtpPort;
            client.Credentials = new NetworkCredential(config.SmtpAccount, config.SmtpPassword);
            client.EnableSsl = true;

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(config.SmtpAccount);
                foreach (var item in receiverMailList)
                {
                    mail.To.Add(item);
                }
                mail.Subject = content.Title;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Body = content.Body;
                mail.BodyEncoding = Encoding.UTF8;
                foreach (var item in ccList)
                {
                    mail.CC.Add(item);
                }
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Dispose();
            }
        }

        /// <summary> 寄送信件 </summary>
        /// <param name="receiverMailList"> 收件人 </param>
        /// <param name="content"> 內容 </param>
        public static void Send(List<string> receiverMailList, EMailContent content)
        {
            List<MailAddress> addressList = receiverMailList.Select(obj => new MailAddress(obj)).ToList();
            Send(addressList, content);
        }

        /// <summary> 寄送信件 </summary>
        /// <param name="receiverMailList"> 收件人 </param>
        /// <param name="ccList"> CC 收件人 </param>
        /// <param name="content"> 內容 </param>
        public static void SendWithCC(List<string> receiverMailList, List<string> ccList, EMailContent content)
        {
            List<MailAddress> addressList = receiverMailList.Select(obj => new MailAddress(obj)).ToList();
            List<MailAddress> ccAddressList = ccList.Select(obj => new MailAddress(obj)).ToList();
            SendWithCC(addressList, ccAddressList, content);
        }


        /// <summary> 寄送信件 </summary>
        /// <param name="receiverMail"> 收件人 </param>
        /// <param name="content"> 內容 </param>
        public static void Send(string receiverMail, EMailContent content)
        {
            List<MailAddress> addressList = new List<MailAddress>() { new MailAddress(receiverMail) };
            Send(addressList, content);
        }
    }
}
