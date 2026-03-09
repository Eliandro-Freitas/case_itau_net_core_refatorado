using CaseItau.Domain.Entities;

namespace CaseItau.Application.DTOs;

public record FundResult(
    string Code,
    string Name,
    string Document,
    decimal? GrossValue,
    FundTypeResult FundType)
{
    public static FundResult ParseToResult(Fund fund)
        => new(
            fund.Code,
            fund.Name,
            fund.Document.Value,
            fund.GrossValue,
            new FundTypeResult(fund.FundType.Id, fund.FundType.Name)
        );

    public static IEnumerable<FundResult> ParseToResult(IEnumerable<Fund> funds)
        => funds.Select(ParseToResult);
}