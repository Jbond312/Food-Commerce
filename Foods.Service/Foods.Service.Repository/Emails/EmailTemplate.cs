using Common.Repository;

namespace Foods.Service.Repository.Emails
{
    public class EmailTemplate : BaseEntity
    {
        public string Key { get; set; }
        public string SourceEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}