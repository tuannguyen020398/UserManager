using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationService.Model.UserModel
{
    public class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        /// <summary>phương thức validator api tạo mới user</summary>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public CreateUserValidator()
        {
            //kiểm tra độ dài name và check null
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Tên không được để trống")
                .MaximumLength(200).WithMessage("Tên không được vượt quá 200 kí tự");
            // kiểm tra phonenumber
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Điện thoại không được để trống")
                .MaximumLength(10).WithMessage("Điện thoại tối đa 10 kí tự");
                //.Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("Phonenumber not valid");
            // kiểm tra ngày sinh không vượt quá 100 tuổi và lớn hơn 18 
            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Sinh nhật không thể lớn hơn 100 năm")
                .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Chưa đủ 18 tuổi");

            //RuleFor(x => x.Gt).NotEmpty().NotNull().WithMessage("Gt is required");
            // kiểm tra email
            RuleFor(x => x.Email)
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email không đúng");
            // kiểm tra tên tài khoản
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("Tên không được để trống")
                .MaximumLength(60).WithMessage("Tên không vượt quá 60 kí tự");
            // kiểm tra mật khẩu
            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("mật khẩu không được để trống")
                .MinimumLength(6).WithMessage("mật khẩu phải từ 6 kí tự trở lên");

        }
    }
}
