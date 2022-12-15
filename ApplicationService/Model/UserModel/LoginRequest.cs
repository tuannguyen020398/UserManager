using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationService.Model.UserModel
{
    /// <summary>khởi tạo dữu liệu đăng nhập</summary>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 12/1/2022 created
    /// </Modified>
    public class LoginRequest
    {
        [Display(Name = "Tài Khoản")]
        public string? UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }
    }
}
