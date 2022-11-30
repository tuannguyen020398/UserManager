using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationService.Model.UserModel
{
    public class LoginRequest
    {
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }
    }
}
