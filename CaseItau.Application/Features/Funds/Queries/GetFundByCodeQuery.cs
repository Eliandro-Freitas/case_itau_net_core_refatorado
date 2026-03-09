using CaseItau.Application.DTOs;
using MediatR;

namespace CaseItau.Application.Features.Funds.Queries;

public class GetFundByCodeQuery : IRequest<FundResult>
{
    public string Code { get; set; }
}