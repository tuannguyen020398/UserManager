using BE.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationService.Model.UserModel
{
    /// <summary>khởi tao dữ liệu để thêm mới user</summary>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 12/1/2022 created
    /// </Modified>
    public class CreateUserModel
    {
        public long Id { get; set; }
        [Display(Name = "Họ Tên")]
        public string? Name { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Display(Name = "Giới tính")]
        public GtStatus? Gt { get; set; }
        [Display(Name = "Tài khoản")]
        public string? UserName { get; set; }
        [Display(Name = "Mật Khẩu")]
        [DataType(DataType.Password)]
        public string? PasswordHash { get; set; }
        [Display(Name = "Nhập lại Mật Khẩu")]
        public string? Passwordagain { get; set; }

    }
}
