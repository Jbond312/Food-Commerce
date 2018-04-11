using System.Threading.Tasks;
using Email.Service.Business.Helper;

namespace Foods.Service.Intercom.Email
{
    public class EmailIntercom : IEmailIntercom
    {
        private readonly EmailHelper _emailHelper;

        public EmailIntercom(EmailHelper emailHelper)
        {
            _emailHelper = emailHelper;
        }

        public async Task Send(EmailTypes emailtype, TransmissionPayload payload)
        {
            await _emailHelper.SendEmail(emailtype, payload);
        }
    }
}