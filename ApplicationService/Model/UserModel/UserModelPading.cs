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
    /// <summary>khởi tạo thuộc tính của user</summary>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 12/1/2022 created
    /// </Modified>
    public class UserModelPading
    {
        [Display(Name = "Tài Khoản")]
        public string? UserName { get; set; }
        public long Id { get; set; }
        [Display(Name = "Họ Tên")]
        public string? Name { get; set; }
        [Display(Name = "Ngày Sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Display(Name = "Giới tính")]
        public SexStatus? Sex { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }
        [Display(Name = "Cập nhật cuối")]
        public DateTime LastUpdate { set; get; }
        [Display(Name = "Trạng thái")]
        public ActiveStatus? Status { get; set; }
    }
}
