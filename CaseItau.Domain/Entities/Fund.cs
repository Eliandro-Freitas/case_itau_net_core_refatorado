using CaseItau.Domain.ValueObjects;

namespace CaseItau.Domain.Entities;

public class Fund
{
    private Fund() { }

    private Fund(string code, string name, Document document, int fundTypeId, decimal? grossValue)
    {
        Code = code;
        Name = name;
        Document = document;
        FundTypeId = fundTypeId;
        GrossValue = grossValue;
    }

    public string Code { get; private set; }
    public string Name { get; private set; }
    public Document Document { get; private set; }
    public FundType FundType { get; private set; }
    public int FundTypeId { get; private set; }
    public decimal? GrossValue { get; private set; }

    public static Fund Create(string code, string name, Document document, int fundTypeId, decimal? grossValue)
        => new(code, name, document, fundTypeId, grossValue);

    public void Update(string name, Document document, int fundTypeId)
    {
        Name = name;
        Document = document;
        FundTypeId = fundTypeId;
    }

    public void Update(decimal? grossValue)
    {
        GrossValue = grossValue;
    }
}