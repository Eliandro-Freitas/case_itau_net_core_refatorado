using CaseItau.Application.DTOs;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Extensions;
using CaseItau.Domain.Interfaces.Repositories;
using CaseItau.Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CaseItau.Application.Commands.UpdateFund;

public class UpdateFundCommandHandler(IFundRepository fundRepository, IFundTypeRepository fundTypeRepository)
    : IRequestHandler<UpdateFundCommand, FundResult>
{

    private readonly IFundRepository _fundRepository = fundRepository;
    private readonly IFundTypeRepository _fundTypeRepository = fundTypeRepository;

    public async Task<FundResult> Handle(UpdateFundCommand request, CancellationToken cancellationToken)
    {
        var fund = await _fundRepository.GetByCode(request.Code, cancellationToken)
            ?? throw new NotFoundException($"Fundo ({request.Code}) não encontrado.");

        var clearedDocument = request.Document.OnlyDigits();
        if (clearedDocument != fund.Document.Value && await _fundRepository.CheckIfExistsByDocument(clearedDocument, cancellationToken))
            throw new ConflictException($"Já existe um fundo com este CNPJ. ({request.Document})");

        if (!await _fundTypeRepository.CheckIfExistsById(request.FundType, cancellationToken))
            throw new ValidationException([new ValidationFailure("FundType", $"Tipo de fundo inválido. ({request.FundType})")]);

        fund.Update(request.Name, Document.Create(request.Document), request.FundType);
        await _fundRepository.Update(fund, cancellationToken);

        return FundResult.ParseToResult(fund);
    }
}