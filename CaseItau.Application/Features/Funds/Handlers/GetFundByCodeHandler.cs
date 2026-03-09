using CaseItau.Application.DTOs;
using CaseItau.Application.Features.Funds.Queries;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces.Repositories;
using MediatR;

namespace CaseItau.Application.Features.Funds.Handlers;

public class GetFundByCodeHandler(IFundRepository fundRepository) : IRequestHandler<GetFundByCodeQuery, FundResult>
{
    private readonly IFundRepository _fundRepository = fundRepository;

    public async Task<FundResult> Handle(GetFundByCodeQuery request, CancellationToken cancellationToken)
    {
        var funds = await _fundRepository.GetByCode(request.Code, cancellationToken)
            ?? throw new NotFoundException($"Fundo com o código ({request.Code}) não encontrado");

        var result = FundResult.ParseToResult(funds);
        return result;
    }
}