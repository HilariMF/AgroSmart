
using Application.Dtos.Email;

namespace Application.Interfaces.Services
{
    public interface IEmailServices
    {
        Task SendAsync(EmailRequest request);
    }
}
