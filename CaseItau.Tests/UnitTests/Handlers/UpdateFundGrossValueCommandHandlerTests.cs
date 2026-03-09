using CaseItau.Application.Commands.UpdateFund;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces.Repositories;
using CaseItau.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace CaseItau.Tests.UnitTests.Handlers;

public class UpdateFundGrossValueCommandHandlerTests
{
    private readonly Mock<IFundRepository> _fundRepository = new();
    private readonly UpdateFundGrossValueCommandHandler _handler;

    public UpdateFundGrossValueCommandHandlerTests()
    {
        _handler = new UpdateFundGrossValueCommandHandler(_fundRepository.Object);
    }

    [Fact]
    public async Task Handle_FundoSemPatrimonio_DeveAdicionarDelta()
    {
        // patrimônio atual = null → (null ?? 0) + 5000 = 5000
        var fund = FundBuilder.Build(code: "ITAU001", grossValue: null);
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        var command = new UpdateFundGrossValueCommand { Code = "ITAU001", GrossValue = 5000m };
        var result = await _handler.Handle(command, CancellationToken.None);

        result.GrossValue.Should().Be(5000m);
        _fundRepository.Verify(r => r.Update(It.IsAny<Fund>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_FundoComPatrimonio_DeveAdicionarDelta()
    {
        // patrimônio atual = 1000, delta = 500 → resultado = 1500
        var fund = FundBuilder.Build(code: "ITAU001", grossValue: 1000m);
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        var command = new UpdateFundGrossValueCommand { Code = "ITAU001", GrossValue = 500m };
        var result = await _handler.Handle(command, CancellationToken.None);

        result.GrossValue.Should().Be(1500m);
    }

    [Fact]
    public async Task Handle_FundoComPatrimonio_DeveSubtrairDeltaNegativo()
    {
        // patrimônio atual = 1000, delta = -200 → resultado = 800
        var fund = FundBuilder.Build(code: "ITAU001", grossValue: 1000m);
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        var command = new UpdateFundGrossValueCommand { Code = "ITAU001", GrossValue = -200m };
        var result = await _handler.Handle(command, CancellationToken.None);

        result.GrossValue.Should().Be(800m);
    }

    [Fact]
    public async Task Handle_FundoNaoEncontrado_DeveLancarNotFoundException()
    {
        _fundRepository.Setup(r => r.GetByCode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Fund?)null);

        var command = new UpdateFundGrossValueCommand { Code = "INEXISTENTE", GrossValue = 5000m };

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
}
