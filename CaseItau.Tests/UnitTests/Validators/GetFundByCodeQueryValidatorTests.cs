using CaseItau.Application.Features.Funds.Queries;
using FluentValidation.TestHelper;

namespace CaseItau.Tests.UnitTests.Validators;

public class GetFundByCodeQueryValidatorTests
{
    private readonly GetFundByCodeQueryValidator _validator = new();

    [Fact]
    public void Validar_CodigoVazio_DeveRetornarErro()
    {
        var query = new GetFundByCodeQuery { Code = "" };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validar_CodigoValido_NaoDeveRetornarErro()
    {
        var query = new GetFundByCodeQuery { Code = "ITAU001" };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
