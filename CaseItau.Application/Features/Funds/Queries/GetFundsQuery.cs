using CaseItau.Application.DTOs;
using MediatR;

namespace CaseItau.Application.Features.Funds.Queries;

public class GetFundsQuery : IRequest<IEnumerable<FundResult>>
{
}