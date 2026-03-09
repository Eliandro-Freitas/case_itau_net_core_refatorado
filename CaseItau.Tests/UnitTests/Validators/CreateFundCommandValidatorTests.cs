using CaseItau.Application.Commands.CreateFund;
using FluentValidation.TestHelper;

namespace CaseItau.Tests.UnitTests.Validators;

public class CreateFundCommandValidatorTests
{
    private readonly CreateFundCommandValidator _validator = new();

    [Fact]
    public void Validar_CodigoVazio_DeveRetornarErro()
    {
        var command = new CreateFundCommand { Code = "", Name = "Fundo Teste", Document = "11222333000181", FundType = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validar_CodigoAcimaDoMaximo_DeveRetornarErro()
    {
        var command = new CreateFundCommand { Code = new string('A', 21), Name = "Fundo Teste", Document = "11222333000181", FundType = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validar_NomeVazio_DeveRetornarErro()
    {
        var command = new CreateFundCommand { Code = "ITAU001", Name = "", Document = "11222333000181", FundType = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validar_NomeAcimaDoMaximo_DeveRetornarErro()
    {
        var command = new CreateFundCommand { Code = "ITAU001", Name = new string('A', 101), Document = "11222333000181", FundType = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validar_DocumentoComMenosDe14Digitos_DeveRetornarErro()
    {
        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "1234567890", FundType = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Document);
    }

    [Fact]
    public void Validar_DocumentoComModulo11Invalido_DeveRetornarErro()
    {
        // 14 dígitos mas dígito verificador errado (11222333000182 ao invés de 11222333000181)
        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "11222333000182", FundType = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Document);
    }

    [Fact]
    public void Validar_TipoFundoZero_DeveRetornarErro()
    {
        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "11222333000181", FundType = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FundType);
    }

    [Fact]
    public void Validar_GrossValueNegativo_DeveRetornarErro()
    {
        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "11222333000181", FundType = 1, GrossValue = -1m };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.GrossValue);
    }

    [Fact]
    public void Validar_DadosValidos_NaoDeveRetornarErro()
    {
        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "11222333000181", FundType = 1, GrossValue = 1000m };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
