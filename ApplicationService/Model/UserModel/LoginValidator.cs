using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Model.UserModel
{
    public class LoginValidator: AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull().WithMessage("Tên không được để trống")
                .MaximumLength(200).WithMessage("Tên không được vượt quá 200 kí tự");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Tên không được để trống")
                .MaximumLength(200).WithMessage("Tên không được vượt quá 200 kí tự");
        }
    }
}
