using FluentValidation;
using TwitchLogs_Web.Models;
namespace TwitchLogs_Web.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty.");
        }
    }
}
