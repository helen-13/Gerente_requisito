using FluentValidation;
using ServidoresAPI.Commands;

namespace ServidoresAPI.Validators;

public class CreateServidorCommandValidator : AbstractValidator<CreateServidorCommand>
{
    public CreateServidorCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .MaximumLength(100);

        RuleFor(x => x.Telefone)
            .MaximumLength(20);

        RuleFor(x => x.OrgaoId)
            .GreaterThan(0);

        RuleFor(x => x.LotacaoId)
            .GreaterThan(0);

        RuleFor(x => x.Sala)
            .MaximumLength(20);
    }
}

public class UpdateServidorCommandValidator : AbstractValidator<UpdateServidorCommand>
{
    public UpdateServidorCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .MaximumLength(100);

        RuleFor(x => x.Telefone)
            .MaximumLength(20);

        RuleFor(x => x.OrgaoId)
            .GreaterThan(0);

        RuleFor(x => x.LotacaoId)
            .GreaterThan(0);

        RuleFor(x => x.Sala)
            .MaximumLength(20);
    }
} 