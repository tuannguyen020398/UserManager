﻿using BE.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Model.UserModel
{
    public class CreateUserModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public GtStatus? Gt { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }

    }
}
