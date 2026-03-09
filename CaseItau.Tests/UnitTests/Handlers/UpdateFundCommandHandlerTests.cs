using CaseItau.Application.Commands.UpdateFund;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces.Repositories;
using CaseItau.Tests.Helpers;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace CaseItau.Tests.UnitTests.Handlers;

public class UpdateFundCommandHandlerTests
{
    private readonly Mock<IFundRepository> _fundRepository = new();
    private readonly Mock<IFundTypeRepository> _fundTypeRepository = new();
    private readonly UpdateFundCommandHandler _handler;

    public UpdateFundCommandHandlerTests()
    {
        _handler = new UpdateFundCommandHandler(_fundRepository.Object, _fundTypeRepository.Object);
    }

    [Fact]
    public async Task Handle_MesmoCnpj_NaoVerificaConflito_DeveAtualizar()
    {
        var fund = FundBuilder.Build(code: "ITAU001", cnpj: "11222333000181");
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);
        _fundTypeRepository.Setup(r => r.CheckIfExistsById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new UpdateFundCommand { Code = "ITAU001", Name = "Novo Nome", Document = "11222333000181", FundType = 1 };
        await _handler.Handle(command, CancellationToken.None);

        _fundRepository.Verify(r => r.Update(It.IsAny<Fund>(), It.IsAny<CancellationToken>()), Times.Once);
        _fundRepository.Verify(r => r.CheckIfExistsByDocument(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CnpjAlteradoSemConflito_DeveAtualizar()
    {
        var fund = FundBuilder.Build(code: "ITAU001", cnpj: "11222333000181");
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);
        _fundRepository.Setup(r => r.CheckIfExistsByDocument("12345678000195", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundTypeRepository.Setup(r => r.CheckIfExistsById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new UpdateFundCommand { Code = "ITAU001", Name = "Novo Nome", Document = "12345678000195", FundType = 1 };
        await _handler.Handle(command, CancellationToken.None);

        _fundRepository.Verify(r => r.Update(It.IsAny<Fund>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_FundoNaoEncontrado_DeveLancarNotFoundException()
    {
        _fundRepository.Setup(r => r.GetByCode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Fund));

        var command = new UpdateFundCommand { Code = "INEXISTENTE", Name = "Novo Nome", Document = "11222333000181", FundType = 1 };

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_CnpjAlteradoEmConflito_DeveLancarConflictException()
    {
        var fund = FundBuilder.Build(code: "ITAU001", cnpj: "11222333000181");
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);
        _fundRepository.Setup(r => r.CheckIfExistsByDocument("12345678000195", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new UpdateFundCommand { Code = "ITAU001", Name = "Novo Nome", Document = "12345678000195", FundType = 1 };

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Handle_TipoFundoInvalido_DeveLancarValidationException()
    {
        var fund = FundBuilder.Build(code: "ITAU001", cnpj: "11222333000181");
        _fundRepository.Setup(r => r.GetByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(fund);
        _fundRepository.Setup(r => r.CheckIfExistsByDocument(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundTypeRepository.Setup(r => r.CheckIfExistsById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new UpdateFundCommand { Code = "ITAU001", Name = "Novo Nome", Document = "11222333000181", FundType = 99 };

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }
}
