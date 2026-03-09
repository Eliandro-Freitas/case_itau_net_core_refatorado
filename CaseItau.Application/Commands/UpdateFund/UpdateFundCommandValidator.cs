using CaseItau.Domain.Extensions;
using FluentValidation;

namespace CaseItau.Application.Commands.UpdateFund;

public class UpdateFundCommandValidator : AbstractValidator<UpdateFundCommand>
{
    public UpdateFundCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("O código do fundo é obrigatório.")
            .MaximumLength(20).WithMessage("O código do fundo deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do fundo é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do fundo deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Document)
            .Must(x => x.OnlyDigits().Length == 14)
            .WithMessage("O CNPJ deve conter 14 dígitos.")
            .Must(x => x.OnlyDigits().IsValidCnpj())
            .WithMessage("O CNPJ informado é inválido.");

        RuleFor(x => x.FundType)
            .GreaterThan(0)
            .WithMessage("O tipo do fundo deve ser informado.");
    }
}