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
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("First name is required")
                .MaximumLength(200).WithMessage("First name can not over 200 characters");
            // kiểm tra phonenumber
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("First phonenumber is required")
                .MaximumLength(10).WithMessage("First phone can not over 10 character");
                //.Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("Phonenumber not valid");
            // kiểm tra ngày sinh không vượt quá 100 tuổi và lớn hơn 18 
            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birthday cannot greater than 100 years")
                .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Birthday cannot greater short 18 years");

            RuleFor(x => x.Gt).NotEmpty().NotNull().WithMessage("Gt is required");
            // kiểm tra email
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format not match");
            // kiểm tra tên tài khoản
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("UserName is requied")
                .MaximumLength(60).WithMessage("UserName can not over 60 charaters");
            // kiểm tra mật khẩu
            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password is at least 6 characters");

        }
    }
}
