using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationService.Model.UserModel
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserModel>
    {
        /// <summary>phương thức validator cập nhật user</summary>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public UpdateUserValidator()
        {
            //kiểm tra tên user
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Tên không đươc để trống")
                .MaximumLength(200).WithMessage("Tên không được vượt quá 200 kí tự");
            //kiểm tra phonenumber
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Điện thoại không được để trống")
                .MaximumLength(10).WithMessage("Điện thoại không được vượt quá 10 kí tự");
                //.Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("Phonenumber noi valid");
            // kiểm tra tuổi không vượt qua 100
            RuleFor(x=>x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Sinh nhật không thể lớn hơn 100 năm")
                .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Chưa đủ 18 tuổi");
            // kiểm tra giới tính
            //RuleFor(x => x.Gt).NotEmpty().NotNull().WithMessage("Gt is required");
            //kiểm tra email
            RuleFor(x => x.Email)
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email không đúng");
        }
    }
}
