using CaseItau.Application.Commands.DeleteFund;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces.Repositories;
using CaseItau.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace CaseItau.Tests.UnitTests.Handlers;

public class DeleteFundByCodeCommandHandlerTests
{
    private readonly Mock<IFundRepository> _fundRepository = new();
    private readonly DeleteFundByCodeCommandHandler _handler;

    public DeleteFundByCodeCommandHandlerTests()
    {
        _handler = new DeleteFundByCodeCommandHandler(_fundRepository.Object);
    }

    [Fact]
    public async Task Handle_FundoExistente_DeveChamarDelete()
    {
        var fund = Fund.Create("ITAU001", "Fundo Teste", Document.Create("11222333000181"), 1, null);
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        await _handler.Handle(new DeleteFundByCodeCommand { Code = "ITAU001" }, CancellationToken.None);

        _fundRepository.Verify(r => r.Delete(It.IsAny<Fund>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_FundoNaoEncontrado_DeveLancarNotFoundException()
    {
        _fundRepository.Setup(r => r.GetByCode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Fund?)null);

        await _handler.Invoking(h => h.Handle(new DeleteFundByCodeCommand { Code = "INEXISTENTE" }, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
}
