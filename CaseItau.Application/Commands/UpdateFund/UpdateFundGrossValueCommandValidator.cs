using FluentValidation;

namespace CaseItau.Application.Commands.UpdateFund;

public class UpdateFundGrossValueCommandValidator : AbstractValidator<UpdateFundGrossValueCommand>
{
    public UpdateFundGrossValueCommandValidator()
    {
        RuleFor(x => x.Code)
           .NotEmpty().WithMessage("O código do fundo é obrigatório.")
           .MaximumLength(20).WithMessage("O código do fundo deve ter no máximo 20 caracteres.");


    }
}