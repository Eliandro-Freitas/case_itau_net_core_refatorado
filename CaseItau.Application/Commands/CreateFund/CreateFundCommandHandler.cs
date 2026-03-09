using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Extensions;
using CaseItau.Domain.Interfaces.Repositories;
using CaseItau.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace CaseItau.Application.Commands.CreateFund;

public class CreateFundCommandHandler(IFundRepository fundRepository, IFundTypeRepository fundTypeRepository) : IRequestHandler<CreateFundCommand>
{
    private readonly IFundRepository _fundRepository = fundRepository;
    private readonly IFundTypeRepository _fundTypeRepository = fundTypeRepository;

    public async Task Handle(CreateFundCommand request, CancellationToken cancellationToken)
    {
        if (await _fundRepository.CheckIfExistsByCode(request.Code, cancellationToken))
            throw new ConflictException($"Já existe um fundo cadastrado com este código. ({request.Code})");

        var clearedDocument = request.Document.OnlyDigits();
        if (await _fundRepository.CheckIfExistsByDocument(clearedDocument, cancellationToken))
            throw new ConflictException($"Já existe um fundo cadastrado com este CNPJ. ({request.Document})");

        if (!await _fundTypeRepository.CheckIfExistsById(request.FundType, cancellationToken))
            throw new ValidationException($"Tipo de fundo inválido. ({request.FundType})");

        var document = Document.Create(request.Document);
        var fund = Fund.Create(request.Code, request.Name, document, request.FundType, request.GrossValue);

        await _fundRepository.Save(fund, cancellationToken);
    }
}