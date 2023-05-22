using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Reflection;

namespace MyUtility
{
    /// <summary>
    /// Summary description for EmailHandler
    /// </summary>
    public class EmailHandler
    {
        public EmailHandler()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool SendEmail(string fullName, string fromEmail, string[] toEmail, string emailSubject, string emailContent, bool isHtmlFormat, string SmtpServer, string EmailSendPassword)
        {
            try
            {
                var smtpClient = new SmtpClient(SmtpServer, 587)
                {
                    Credentials = new NetworkCredential(fromEmail, EmailSendPassword),
                    EnableSsl = true
                };

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail, fullName);
                if (toEmail.Length > 0)
                {
                    for (int i = 0; i < toEmail.Length; i++)
                    {
                        message.To.Add(new MailAddress(toEmail[i]));
                    }
                }
                message.Subject = emailSubject;
                message.Body = emailContent;
                message.IsBodyHtml = isHtmlFormat;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.UTF8;


                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class SmtpClientEx : SmtpClient
    {
        private static readonly FieldInfo localHostName = GetLocalHostNameField();

        public SmtpClientEx(string host, int port)
            : base(host, port)
        {
            Initialize();
        }

        public SmtpClientEx(string host)
            : base(host)
        {
            Initialize();
        }

        public SmtpClientEx()
        {
            Initialize();
        }

        public string LocalHostName
        {
            get
            {
                if (null == localHostName) return null;
                return (string)localHostName.GetValue(this);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (null != localHostName)
                {
                    localHostName.SetValue(this, value);
                }
            }
        }

        private static FieldInfo GetLocalHostNameField()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo result = typeof(SmtpClient).GetField("clientDomain", flags);
            if (null == result) result = typeof(SmtpClient).GetField("localHostName", flags);
            return result;
        }

        private void Initialize()
        {
            IPGlobalProperties ip = IPGlobalProperties.GetIPGlobalProperties();
            //if (!string.IsNullOrEmpty(ip.HostName) && !string.IsNullOrEmpty(ip.DomainName))
            //{
            //    this.LocalHostName = ip.HostName + "." + ip.DomainName;
            //}
            this.LocalHostName = "google.com";
        }
    }
}