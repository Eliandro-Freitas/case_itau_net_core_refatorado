using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces.Repositories;
using MediatR;

namespace CaseItau.Application.Commands.DeleteFund;

public class DeleteFundByCodeCommandHandler(IFundRepository fundRepository) : IRequestHandler<DeleteFundByCodeCommand>
{
    private readonly IFundRepository _fundRepository = fundRepository;

    public async Task Handle(DeleteFundByCodeCommand request, CancellationToken cancellationToken)
    {
        var fund = await _fundRepository.GetByCode(request.Code, cancellationToken) ??
            throw new NotFoundException($"Fundo com o código: ({request.Code}) não encontrado.");

        await _fundRepository.Delete(fund, cancellationToken);
    }
}