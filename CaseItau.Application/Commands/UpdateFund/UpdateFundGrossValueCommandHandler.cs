using CaseItau.Application.DTOs;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces.Repositories;
using MediatR;

namespace CaseItau.Application.Commands.UpdateFund;

public class UpdateFundGrossValueCommandHandler(IFundRepository fundRepository) : IRequestHandler<UpdateFundGrossValueCommand, FundResult>
{
    private readonly IFundRepository _fundRepository = fundRepository;

    public async Task<FundResult> Handle(UpdateFundGrossValueCommand request, CancellationToken cancellationToken)
    {
        var fund = await _fundRepository.GetByCode(request.Code, cancellationToken)
            ?? throw new NotFoundException($"Fundo ({request.Code}) não encontrado.");

        var newGrossValue = request.GrossValue.HasValue
            ? (fund.GrossValue ?? 0m) + request.GrossValue.Value
            : fund.GrossValue;

        fund.Update(newGrossValue);
        await _fundRepository.Update(fund, cancellationToken);

        return FundResult.ParseToResult(fund);
    }
}