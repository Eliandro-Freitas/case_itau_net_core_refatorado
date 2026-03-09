using CaseItau.Application.Features.Funds.Handlers;
using CaseItau.Application.Features.Funds.Queries;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces.Repositories;
using CaseItau.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace CaseItau.Tests.UnitTests.Handlers;

public class GetFundByCodeHandlerTests
{
    private readonly Mock<IFundRepository> _fundRepository = new();
    private readonly GetFundByCodeHandler _handler;

    public GetFundByCodeHandlerTests()
    {
        _handler = new GetFundByCodeHandler(_fundRepository.Object);
    }

    [Fact]
    public async Task Handle_FundoExistente_DeveRetornarFundResult()
    {
        var fund = FundBuilder.Build(code: "ITAU001", name: "Fundo Teste");
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        var result = await _handler.Handle(new GetFundByCodeQuery { Code = "ITAU001" }, CancellationToken.None);

        result.Code.Should().Be("ITAU001");
        result.Name.Should().Be("Fundo Teste");
        result.Document.Should().Be("11222333000181");
    }

    [Fact]
    public async Task Handle_FundoNaoEncontrado_DeveLancarNotFoundException()
    {
        _fundRepository.Setup(r => r.GetByCode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Fund?)null);

        await _handler.Invoking(h => h.Handle(new GetFundByCodeQuery { Code = "INEXISTENTE" }, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
}
