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
    public class UserModelPading:ViewUserModel
    {
        [Display(Name = "Tài Khoản")]
        public string? UserName { get; set; }
    }
}
