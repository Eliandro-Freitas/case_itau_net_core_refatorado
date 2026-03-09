using CaseItau.Application.DTOs;
using MediatR;

namespace CaseItau.Application.Commands.UpdateFund;

public class UpdateFundGrossValueCommand : IRequest<FundResult>
{
    public string Code { get; set; }
    public decimal? GrossValue { get; set; }
}