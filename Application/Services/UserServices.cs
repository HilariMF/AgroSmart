using Application.Dtos.Account;
using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.ViewModels.User;
using AutoMapper;
using System.Reflection;

namespace Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;

        public UserServices(IAccountServices accountServices, IMapper mapper)
        {
            _accountServices = accountServices;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountServices.AuthenticateAsync(loginRequest);
            return userResponse;
        }

        public async Task SignOutAsync()
        {
            await _accountServices.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            var request = _mapper.Map<RegisterRequest>(vm);

            if (vm.SelectRole == ((int)Roles.Client))
            {
                return await _accountServices.RegisterAsync(request, origin);
            }

            return await _accountServices.RegisterAsync(request, null);
        }

        public async Task<string> ConfirmEmailAsync(string userId, string origin)
        {
            return await _accountServices.ConfirmAccountAsync(userId, origin);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotPasswordRequest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountServices.ForgotPasswordAsync(forgotPasswordRequest, origin);

        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetPasswordRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountServices.ResetPasswordAsync(resetPasswordRequest);
        }
    }
}
