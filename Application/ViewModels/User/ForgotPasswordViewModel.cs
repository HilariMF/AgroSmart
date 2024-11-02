

namespace Application.ViewModels.User
{
    public class ForgotPasswordViewModel
    {
        public string? Email { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
