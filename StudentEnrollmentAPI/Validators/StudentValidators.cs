using FluentValidation;
using StudentEnrollmentAPI.DTOs;

namespace StudentEnrollmentAPI.Validators
{
    public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .Length(2, 100).WithMessage("Nome deve ter entre 2 e 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter um formato válido")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

            RuleFor(x => x.RA)
                .NotEmpty().WithMessage("RA é obrigatório")
                .MaximumLength(20).WithMessage("RA deve ter no máximo 20 caracteres");

            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage("CPF é obrigatório")
                .Length(11).WithMessage("CPF deve conter exatamente 11 dígitos")
                .Matches(@"^\d{11}$").WithMessage("CPF deve conter apenas números")
                .Must(BeValidCPF).WithMessage("CPF inválido");
        }

        private bool BeValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                return false;

            //verifica se todos os dígitos são iguais
            if (cpf.All(c => c == cpf[0]))
                return false;

            //validação do CPF
            var sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * (10 - i);

            var remainder = sum % 11;
            var digit1 = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cpf[9].ToString()) != digit1)
                return false;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            var digit2 = remainder < 2 ? 0 : 11 - remainder;

            return int.Parse(cpf[10].ToString()) == digit2;
        }
    }

    public class StudentUpdateDtoValidator : AbstractValidator<StudentUpdateDto>
    {
        public StudentUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .Length(2, 100).WithMessage("Nome deve ter entre 2 e 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter um formato válido")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");
        }
    }
}
