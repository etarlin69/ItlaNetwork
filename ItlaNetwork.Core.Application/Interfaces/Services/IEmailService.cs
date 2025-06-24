using ItlaNetwork.Core.Application.DTOs.Email;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}