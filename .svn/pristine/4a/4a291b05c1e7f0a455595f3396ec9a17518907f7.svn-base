using FairPos.Epylion.Models.Setups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace FairPos.Epyllion.Repository
{
    public interface IEmailHelperRepository
    {
        bool EmailSender(string message, List<string> ToWhom, List<string> ToWhomBcc, string Subject, Attachment attachment, bool IsBodyHtml = false);
        Attachment MakeAttachment(byte[] obj, string att_name, string exten);
    }
    public class EmailHelperRepository : BaseRepository, IEmailHelperRepository
    {
        public EmailHelperRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {

        }
        private GlobalSetup GetSoftwareSetting()
        {
            string sql = "select * from GlobalSetup";
            var response = _dal.Select<GlobalSetup>(sql, ref msg).FirstOrDefault();
            if (response == null)
            {
                return new GlobalSetup();
            }
            return response;
        }
        public bool EmailSender(string message, List<string> ToWhom, List<string> ToWhomBcc, string Subject, Attachment attachment, bool IsBodyHtml = false)
        {
            GlobalSetup globalSetup = GetSoftwareSetting();
            using (MailMessage mailMessage = new())
            {
                using (SmtpClient smtpClient = new(globalSetup.smtpAddress, globalSetup.smtpPort))
                {
                    try
                    {
                        smtpClient.EnableSsl = globalSetup.EnableSsl;
                        foreach (string EmailAddress in ToWhom)
                        {
                            mailMessage.To.Add(EmailAddress);
                        }

                        mailMessage.From = new MailAddress(globalSetup.SenderEmail);
                        mailMessage.Subject = Subject;

                        if (ToWhomBcc != null && ToWhomBcc.Count > 0)
                        {
                            foreach (string item in ToWhomBcc)
                            {
                                mailMessage.Bcc.Add(item);
                            }
                        }

                        mailMessage.IsBodyHtml = true;
                        mailMessage.Body = message;

                        if (attachment != null)
                        {
                            mailMessage.Attachments.Add(attachment);
                        }

                        smtpClient.Credentials = new System.Net.NetworkCredential(globalSetup.SenderEmail, globalSetup.SenderPass);

                        smtpClient.Send(mailMessage);
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                    finally
                    {
                        smtpClient.Dispose();
                    }
                }
            }
        }
        public Attachment MakeAttachment(byte[] obj, string att_name, string exten)
        {
            return new Attachment(new MemoryStream(obj), att_name + "." + exten, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }
    }
}
