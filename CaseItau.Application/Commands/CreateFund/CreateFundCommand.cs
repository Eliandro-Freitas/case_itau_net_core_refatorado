using MediatR;

namespace CaseItau.Application.Commands.CreateFund;

public class CreateFundCommand : IRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public int FundType { get; set; }
    public decimal? GrossValue { get; set; }
}