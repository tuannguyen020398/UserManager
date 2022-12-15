using ApplicationService.Resource;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Model.UserModel
{
    public class UserModelPadingVailidator:AbstractValidator<FilterUserResource>
    {
        public UserModelPadingVailidator()
        {
            RuleFor(x => x.StartDob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Sinh nhật không thể lớn hơn 100 năm");
            RuleFor(x => x.EndDob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Sinh nhật không thể lớn hơn 100 năm");
        }           
    }
}
