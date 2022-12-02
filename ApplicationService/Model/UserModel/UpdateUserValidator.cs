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
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("First name is required")
                .MaximumLength(200).WithMessage("First name can not over 200 characters");
            //kiểm tra phonenumber
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("First phonenumber is required")
                .MaximumLength(10).WithMessage("First phone can not over 10 character");
                //.Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("Phonenumber noi valid");
            // kiểm tra tuổi không vượt qua 100
            RuleFor(x=>x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birthday cannot greater than 100 years")
                .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Birthday cannot greater short 18 years");
            // kiểm tra giới tính
            RuleFor(x => x.Gt).NotEmpty().NotNull().WithMessage("Gt is required");
            //kiểm tra email
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format not match");
        }
    }
}
