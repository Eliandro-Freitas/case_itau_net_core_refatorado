using CaseItau.Application.Features.Funds.Handlers;
using CaseItau.Application.Features.Funds.Queries;
using CaseItau.Domain.Interfaces.Repositories;
using CaseItau.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace CaseItau.Tests.UnitTests.Handlers;

public class GetFundsHandlerTests
{
    private readonly Mock<IFundRepository> _fundRepository = new();
    private readonly GetFundsHandler _handler;

    public GetFundsHandlerTests()
    {
        _handler = new GetFundsHandler(_fundRepository.Object);
    }

    [Fact]
    public async Task Handle_ExistemFundos_DeveRetornarListaDeResults()
    {
        var funds = new[]
        {
            FundBuilder.Build(code: "ITAU001", name: "Fundo A"),
            FundBuilder.Build(code: "ITAU002", name: "Fundo B", cnpj: "12345678000195")
        };

        _fundRepository.Setup(r => r.Get(It.IsAny<CancellationToken>())).ReturnsAsync(funds);

        var result = await _handler.Handle(new GetFundsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_SemFundos_DeveRetornarListaVazia()
    {
        _fundRepository.Setup(r => r.Get(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var result = await _handler.Handle(new GetFundsQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}
