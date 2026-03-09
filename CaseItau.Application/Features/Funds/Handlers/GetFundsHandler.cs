using CaseItau.Application.DTOs;
using CaseItau.Application.Features.Funds.Queries;
using CaseItau.Domain.Interfaces.Repositories;
using MediatR;

namespace CaseItau.Application.Features.Funds.Handlers;

public class GetFundsHandler(IFundRepository fundRepository) : IRequestHandler<GetFundsQuery, IEnumerable<FundResult>>
{
    private readonly IFundRepository _fundRepository = fundRepository;

    public async Task<IEnumerable<FundResult>> Handle(GetFundsQuery request, CancellationToken cancellationToken)
    {
        var funds = await _fundRepository.Get(cancellationToken);
        if (funds is null)
        {
            return [];
        }

        var result = FundResult.ParseToResult(funds);
        return result;
    }
}