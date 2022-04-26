using FairPos.Epyllion.Repository;
using System.Collections.Generic;
using System.Net.Mail;

namespace FairPos.Epylion.Service
{
    public interface IEmailHelperService
    {
        bool EmailSender(string message, List<string> ToWhom, List<string> ToWhomBcc, string Subject, Attachment attachment, bool IsBodyHtml = false);
        Attachment MakeAttachment(byte[] obj, string att_name, string exten);
    }
    public class EmailHelperService : IEmailHelperService
    {
        readonly IEmailHelperRepository repository;
        public EmailHelperService(IEmailHelperRepository _repo)
        {
            repository = _repo;
        }
        public bool EmailSender(string message, List<string> ToWhom, List<string> ToWhomBcc, string Subject, Attachment attachment, bool IsBodyHtml = false)
        {
            return repository.EmailSender(message, ToWhom, ToWhomBcc, Subject, attachment, IsBodyHtml);
        }
        public Attachment MakeAttachment(byte[] obj, string att_name, string exten)
        {
            return repository.MakeAttachment(obj, att_name, exten);
        }
    }
}