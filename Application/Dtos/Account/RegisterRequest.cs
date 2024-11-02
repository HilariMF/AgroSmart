using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.Account
{
    public class RegisterRequest
    {

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string ImageUser { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        [JsonIgnore]
        public bool IsActive { get; set; }
        [JsonIgnore]
        public int SelectRole { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult("Las contraseñas no coinciden.", new[] { nameof(Password), nameof(ConfirmPassword) });
            }
        }
    }
}
