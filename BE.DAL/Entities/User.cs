using BE.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public SexStatus? Sex { get; set; }
        public DateTime DateCreated { set; get; }
        public DateTime LastUpdate { set; get; }
        public ActiveStatus? Status { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
    }
}
