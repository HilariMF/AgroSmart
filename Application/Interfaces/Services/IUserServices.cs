using Application.Dtos.Account;
using Application.ViewModels.User;

namespace Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<string> ConfirmEmailAsync(string userId, string origin);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task SignOutAsync();
    }
}