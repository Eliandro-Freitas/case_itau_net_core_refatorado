using CaseItau.Domain.ValueObjects;
using FluentAssertions;

namespace CaseItau.Tests.UnitTests.Domain.ValueObjects;

public class DocumentTests
{
    [Theory]
    [InlineData("11222333000181")]
    [InlineData("11.222.333/0001-81")]
    [InlineData("11 222 333 0001 81")]
    public void Create_WithValidCnpj_ReturnsDocument(string cnpj)
    {
        var document = Document.Create(cnpj);

        document.Should().NotBeNull();
        document.Value.Should().Be(new string(cnpj.Where(char.IsDigit).ToArray()));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithNullOrEmptyCnpj_ThrowsArgumentException(string cnpj)
    {
        var action = () => Document.Create(cnpj);

        action.Should().Throw<ArgumentException>()
            .WithMessage("O CNPJ é obrigatório.");
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("123456789012345")]
    [InlineData("abc")]
    public void Create_WithInvalidLengthCnpj_ThrowsArgumentException(string cnpj)
    {
        var action = () => Document.Create(cnpj);

        action.Should().Throw<ArgumentException>()
            .WithMessage("O CNPJ deve conter exatamente 14 dígitos numéricos.");
    }

    [Theory]
    [InlineData("00000000000000")]
    [InlineData("11111111111111")]
    [InlineData("12345678901234")]
    public void Create_WithInvalidCheckDigit_ThrowsArgumentException(string cnpj)
    {
        var action = () => Document.Create(cnpj);

        action.Should().Throw<ArgumentException>()
            .WithMessage("O CNPJ informado é inválido.");
    }

    [Fact]
    public void Equals_TwoDocumentsWithSameCnpj_AreEqual()
    {
        var documentA = Document.Create("11222333000181");
        var documentB = Document.Create("11222333000181");

        documentA.Should().Be(documentB);
    }

    [Fact]
    public void Equals_TwoDocumentsWithDifferentCnpj_AreNotEqual()
    {
        var documentA = Document.Create("11222333000181");
        var documentB = Document.Create("04961224000110");

        documentA.Should().NotBe(documentB);
    }

    [Fact]
    public void ToString_WithFormattedCnpj_ReturnsOnlyDigits()
    {
        var document = Document.Create("11.222.333/0001-81");

        document.ToString().Should().Be("11222333000181");
    }

    [Fact]
    public void GetHashCode_TwoDocumentsWithSameCnpj_ReturnSameHash()
    {
        var documentA = Document.Create("11222333000181");
        var documentB = Document.Create("11222333000181");

        documentA.GetHashCode().Should().Be(documentB.GetHashCode());
    }
}