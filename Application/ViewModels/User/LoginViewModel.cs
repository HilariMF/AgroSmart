using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre del usuario")]
        [DataType(DataType.Text)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar la contrasenia")]
        [DataType(DataType.Text)]
        public string? Password { get; set; }

        public bool HasError {  get; set; }
        public string Error { get; set; }
    }
}
