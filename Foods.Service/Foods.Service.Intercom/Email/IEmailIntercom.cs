using System.Threading.Tasks;
using Email.Service.Business.Helper;

namespace Foods.Service.Intercom.Email
{
    public interface IEmailIntercom
    {
        Task Send(EmailTypes emailtype, TransmissionPayload payload);
    }
}