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
    /// <summary>khởi tạo dữ liệu cập nhật dữ liệu người dùng</summary>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 12/1/2022 created
    /// </Modified>
    public class UpdateUserModel
    {
        public long Id { get; set; }
        [Display(Name = "Họ Tên")]
        public string? Name { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Display(Name = "số điện thoại")]
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
