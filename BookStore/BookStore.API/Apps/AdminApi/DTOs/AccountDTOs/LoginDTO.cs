using FluentValidation;

namespace BookStore.Api.Apps.AdminApi.DTOs.AccountDTOs
{
    public class LoginDTO
    {
        public string UserName { get; set; }    
        public string Password { get; set; }

    }
    public class LoginDTOValidator:AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(50).MinimumLength(3).WithMessage("Adin uzunluqu min 4 max 50");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(25).WithMessage("Sifre uzunluqu min 6 max 25");
        }
    }
}
