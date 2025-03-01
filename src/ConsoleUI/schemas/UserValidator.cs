using FluentValidation;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;

namespace Hotel.src.ConsoleUI.schemas
{
    class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {

            RuleFor(u => u.NAME)
                .NotEmpty().WithMessage("El nombre es obligatorio");

            RuleFor(u => u.EMAIL)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo electrónico no es válido");

            RuleFor(u => u.PASSWORD)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(5).WithMessage("La contraseña debe tener al menos 5 caracteres");

            RuleFor(u => u.ROLE)
                .Must(BeAValidRole).WithMessage("El rol no es valido");
        }

        private bool BeAValidRole(RoleUser role) => role == RoleUser.Admin || role == RoleUser.User;

    }
}
