using CaseItau.Application.DTOs;
using MediatR;

namespace CaseItau.Application.Commands.UpdateFund;

public class UpdateFundCommand : IRequest<FundResult>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public int FundType { get; set; }
}