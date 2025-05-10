using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SmartLink.Application.DTOs.User;
using SmartLink.Domain.Entities.Identity;

namespace SmartLink.Application.Validators
{
    public class UserValidator : AbstractValidator<UserRegisterDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Length(6);
        }
    }
}
