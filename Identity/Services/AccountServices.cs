using Application.Dtos.Account;
using Application.Dtos.Email;
using Application.Enums;
using Application.Interfaces.Services;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;



namespace Identity.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailServices _emailServices;

        public AccountServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailServices emailServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        #region "Login"
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No hay ninguna cuenta registrada con {request.Email}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Credenciales Invalidas para {request.Email}";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"La Cuenta {request.Email} no esta confirmada";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }
        #endregion

        #region "Registrar"
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, string? origin)
        {
            RegisterResponse response = new()
            {

                HasError = false
            };

            var userName = await _userManager.FindByNameAsync(request.UserName);
            if (userName != null)
            {
                response.HasError = true;
                response.Error = $"El nombre de usuario '{request.UserName}' ya esta en uso";
                return response;
            }


            var userEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userEmail != null)
            {
                response.HasError = true;
                response.Error = $"El correo '{request.Email}' ya esta en uso";
                return response;
            }

            //Manejo de variables
            Dictionary<int, (bool emailConfirmed, bool isActive)> roleProperties = new()
            {
                { (int)Roles.Client, (true, false) },
                { (int)Roles.Admin, (true, true) }
            };

            (bool emailConfirmed, bool isActive) = roleProperties.TryGetValue(request.SelectRole, out (bool emailConfirmed, bool isActive) value) ? value : (false, false);

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ImageUser = request.ImageUser,
                IsActive = isActive,
                EmailConfirmed = emailConfirmed
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            response.IdUser = user.Id;
            if (result.Succeeded)
            {

                switch (request.SelectRole)
                {
                    case (int)Roles.Client:
                        await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
                        user.EmailConfirmed = false;
                        var verificationUrl = await SendVerificationEmailUrl(user, origin);
                        await _emailServices.SendAsync(new EmailRequest
                        {
                            To = user.Email,
                            Body = $"¡Por favor, haga clic en este enlace para verficar su cuenta! {verificationUrl}",
                            Subject = "Confirme su registro"
                        });
                        break;
                    case (int)Roles.Admin:
                        await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                        break;
                }

            }
            else
            {
                response.HasError = true;
                response.Error = $"Error al registrar al usuario";
                return response;
            }

            return response;
        }

        #region "Confirmar cuenta"
        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No hay cuentas registradas para este usuario";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Cuenta confirmada para {user.Email}. Ahora puedes usar la app";
            }
            else
            {
                return $"Ha ocurrido un error confirmando {user.Email}";
            }
        }

        #region "Notificacion por correo"
        private async Task<string> SendVerificationEmailUrl(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }
        #endregion

        #endregion
        #endregion

        #region "Operaciones para contraseña"

        #region "Olvide mi contraseña"
        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var account = await _userManager.FindByEmailAsync(request.Email);
            if (account == null)
            {
                response.HasError = true;
                response.Error = $"No Hay cuentas registradas con {request.Email}";
                return response;
            }


            var verificationUri = await SendForgotPasswordUrl(account, origin);
            await _emailServices.SendAsync(new EmailRequest()
            {
                To = account.Email,
                Body = $"Restablece tu contraseña visitando este enlace{verificationUri}",
                Subject = "Reset Password"
            });


            return response;
        }
        #endregion

        #region "Restablecer contraseña"
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var account = await _userManager.FindByEmailAsync(request.Email);
            if (account == null)
            {
                response.HasError = true;
                response.Error = $"No hay cuentas registradas con {request.Email}";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ConfirmEmailAsync(account, request.Token);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Ha ocurrido un error mientras se restablecia la contraseña";
                return response;
            }

            return response;

        }

        private async Task<string> SendForgotPasswordUrl(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }
        #endregion

        #endregion

        #region "Cerrar Sesion"
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion




 

    }
}
