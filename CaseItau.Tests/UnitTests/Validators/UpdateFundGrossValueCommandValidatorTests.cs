using CaseItau.Application.Commands.UpdateFund;
using FluentValidation.TestHelper;

namespace CaseItau.Tests.UnitTests.Validators;

public class UpdateFundGrossValueCommandValidatorTests
{
    private readonly UpdateFundGrossValueCommandValidator _validator = new();

    [Fact]
    public void Validar_CodigoVazio_DeveRetornarErro()
    {
        var command = new UpdateFundGrossValueCommand { Code = "", GrossValue = 1000m };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validar_GrossValueNegativo_DevePermitirSubtracao()
    {
        // Valor negativo é válido — representa uma subtração do patrimônio atual
        var command = new UpdateFundGrossValueCommand { Code = "ITAU001", GrossValue = -0.01m };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_GrossValueNulo_NaoDeveRetornarErro()
    {
        // GrossValue é opcional — null significa sem alteração no patrimônio
        var command = new UpdateFundGrossValueCommand { Code = "ITAU001", GrossValue = null };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_DadosValidos_NaoDeveRetornarErro()
    {
        var command = new UpdateFundGrossValueCommand { Code = "ITAU001", GrossValue = 5000m };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
