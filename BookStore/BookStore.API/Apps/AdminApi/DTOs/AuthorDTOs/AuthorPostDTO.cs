using FluentValidation;
using System;

namespace BookStore.Api.DTOs.AuthorDTOs
{
    public class AuthorPostDTO
    {
        public string FullName { get; set; }
        public int BornYear { get; set; }

    }
    public class AuthorPostDTOValidator:AbstractValidator<AuthorPostDTO>
    {
        public AuthorPostDTOValidator()
        {
            RuleFor(x => x.FullName).NotNull().MaximumLength(50).MinimumLength(8).WithMessage("Ad və Soyad Birlikdə minimum 8 max 50 simvoldan ibarət olmalıdır");
            RuleFor(x => x.BornYear).NotNull().LessThanOrEqualTo(DateTime.Now.Year - 18).WithMessage("Doğum ili 2004 dən aşağı ola bilməz");

        }
    }
}
