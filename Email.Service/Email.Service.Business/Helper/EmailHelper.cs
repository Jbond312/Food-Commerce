using System.Linq;
using System.Threading.Tasks;
using Common.Repository;
using Foods.Service.Repository.Emails;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SparkPost;

namespace Email.Service.Business.Helper
{
    public class EmailHelper
    {
        private readonly string _clientKey;
        private readonly IRepository<EmailTemplate> _templateRepo;

        public EmailHelper(IRepository<EmailTemplate> templateRepo, IConfiguration config)
        {
            _templateRepo = templateRepo;
            _clientKey = config["ApiKeys:SparkPost"];
        }

        public async Task SendEmail(EmailTypes emailtype, TransmissionPayload payload)
        {
            var template = await GetTemplate(emailtype.ToString());

            var transmission = new Transmission
            {
                Recipients = payload.Destinations.Select(x => new Recipient
                {
                    Address = new Address(x.DestinationEmail),
                    SubstitutionData = x.SubstitutionData
                }).ToList(),
                Content =
                {
                    Html = template.Body,
                    From = new Address(template.SourceEmail),
                    Subject = template.Subject,
                    Attachments = payload.Attachments.ToList()
                }
            };
            var client = new Client(_clientKey);
            await client.Transmissions.Send(transmission);
        }

        private async Task<EmailTemplate> GetTemplate(string key)
        {
            var filterBuilder = new FilterDefinitionBuilder<EmailTemplate>();
            var filter = filterBuilder.Where(x => x.Key == key);
            var result = await _templateRepo.GetAll(filter);
            return result.FirstOrDefault();
        }
    }
}