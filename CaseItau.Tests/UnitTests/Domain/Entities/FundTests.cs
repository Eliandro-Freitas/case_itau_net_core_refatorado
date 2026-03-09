using CaseItau.Domain.Entities;
using CaseItau.Domain.ValueObjects;
using FluentAssertions;

namespace CaseItau.Tests.UnitTests.Domain.Entities;

public class FundTests
{
    private static Document _validDocument => Document.Create("52917370000191");

    [Fact]
    public void Create_WithValidData_ReturnsFund()
    {
        var fund = Fund.Create("ITAURF123", "Itau Renda Fixa", _validDocument, 1, 1500000m);

        fund.Should().NotBeNull();
        fund.Code.Should().Be("ITAURF123");
        fund.Name.Should().Be("Itau Renda Fixa");
        fund.Document.Should().Be(_validDocument);
        fund.FundTypeId.Should().Be(1);
        fund.GrossValue.Should().Be(1500000m);
    }

    [Fact]
    public void Create_WithNullGrossValue_ReturnsFundWithNullGrossValue()
    {
        var fund = Fund.Create("ITAURF123", "Itau Renda Fixa", _validDocument, 1, null);

        fund.GrossValue.Should().BeNull();
    }

    [Fact]
    public void Update_WithValidData_UpdatesNameDocumentAndFundType()
    {
        var fund = Fund.Create("ITAURF123", "Itau Renda Fixa", _validDocument, 1, 1500000m);
        var newDocument = Document.Create("52917370000191");

        fund.Update("Itau Acoes", newDocument, 2);

        fund.Name.Should().Be("Itau Acoes");
        fund.Document.Should().Be(newDocument);
        fund.FundTypeId.Should().Be(2);
    }

    [Fact]
    public void Update_WithValidData_DoesNotChangeCodeAndGrossValue()
    {
        var fund = Fund.Create("ITAURF123", "Itau Renda Fixa", _validDocument, 1, 1500000m);

        fund.Update("Itau Acoes", _validDocument, 2);

        fund.Code.Should().Be("ITAURF123");
        fund.GrossValue.Should().Be(1500000m);
    }

    [Fact]
    public void UpdateGrossValue_WithPositiveValue_UpdatesGrossValue()
    {
        var fund = Fund.Create("ITAURF123", "Itau Renda Fixa", _validDocument, 1, 1500000m);

        fund.Update(2500000m);

        fund.GrossValue.Should().Be(2500000m);
    }

    [Fact]
    public void UpdateGrossValue_WithNullValue_SetsGrossValueToNull()
    {
        var fund = Fund.Create("ITAURF123", "Itau Renda Fixa", _validDocument, 1, 1500000m);

        fund.Update(null);

        fund.GrossValue.Should().BeNull();
    }

    [Fact]
    public void UpdateGrossValue_WithZeroValue_UpdatesGrossValue()
    {
        var fund = Fund.Create("ITAURF123", "Itau Renda Fixa", _validDocument, 1, 1500000m);

        fund.Update(0m);

        fund.GrossValue.Should().Be(0m);
    }

    [Fact]
    public void UpdateGrossValue_DoesNotChangeOtherProperties()
    {
        var fund = Fund.Create("ITAURF123", "Itau Renda Fixa", _validDocument, 1, 1500000m);

        fund.Update(2500000m);

        fund.Code.Should().Be("ITAURF123");
        fund.Name.Should().Be("Itau Renda Fixa");
        fund.Document.Should().Be(_validDocument);
        fund.FundTypeId.Should().Be(1);
    }
}