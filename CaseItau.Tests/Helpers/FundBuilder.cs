using CaseItau.Domain.Entities;
using CaseItau.Domain.ValueObjects;

namespace CaseItau.Tests.Helpers;

internal static class FundBuilder
{
    internal static Fund Build(
        string code = "ITAU001",
        string name = "Fundo Teste",
        string cnpj = "11222333000181",
        int fundTypeId = 1,
        string fundTypeName = "RENDA_FIXA",
        decimal? grossValue = null)
    {
        var document = Document.Create(cnpj);
        var fund = Fund.Create(code, name, document, fundTypeId, grossValue);

        var fundType = FundType.Create(fundTypeId, fundTypeName);

        typeof(Fund)
            .GetProperty(nameof(Fund.FundType))!
            .SetValue(fund, fundType);

        return fund;
    }
}