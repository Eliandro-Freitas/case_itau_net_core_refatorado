using CaseItau.Application.Commands.DeleteFund;
using FluentValidation.TestHelper;

namespace CaseItau.Tests.UnitTests.Validators;

public class DeleteFundByCodeCommandValidatorTests
{
    private readonly DeleteFundByCodeCommandValidator _validator = new();

    [Fact]
    public void Validar_CodigoVazio_DeveRetornarErro()
    {
        var command = new DeleteFundByCodeCommand { Code = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validar_CodigoAcimaDoMaximo_DeveRetornarErro()
    {
        var command = new DeleteFundByCodeCommand { Code = new string('A', 21) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validar_CodigoValido_NaoDeveRetornarErro()
    {
        var command = new DeleteFundByCodeCommand { Code = "ITAU001" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
