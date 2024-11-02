using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Account
{
    public class RegisterResponse
    {
        public string? IdUser { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}
