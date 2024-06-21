using FluentValidation;
using NotesApiNext.Models.User;

namespace NotesApiNext.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotNull();
            RuleFor(user => user.Email).NotNull();
            RuleFor(user => user.Password).NotNull();
        }
    }
}
