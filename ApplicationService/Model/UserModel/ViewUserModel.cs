﻿using BE.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationService.Model.UserModel
{
    public class ViewUserModel
    {
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
        public GtStatus? Gt { get; set; }
    }
}
