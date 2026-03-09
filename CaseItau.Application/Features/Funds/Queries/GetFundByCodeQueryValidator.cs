using FluentValidation;

namespace CaseItau.Application.Features.Funds.Queries;

public class GetFundByCodeQueryValidator : AbstractValidator<GetFundByCodeQuery>
{
    public GetFundByCodeQueryValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("O código do fundo é obrigatório.");
    }
}