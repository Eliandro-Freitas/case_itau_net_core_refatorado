using CaseItau.Application.Commands.CreateFund;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces.Repositories;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace CaseItau.Tests.UnitTests.Handlers;

public class CreateFundCommandHandlerTests
{
    private readonly Mock<IFundRepository> _fundRepository = new();
    private readonly Mock<IFundTypeRepository> _fundTypeRepository = new();
    private readonly CreateFundCommandHandler _handler;

    public CreateFundCommandHandlerTests()
    {
        _handler = new CreateFundCommandHandler(_fundRepository.Object, _fundTypeRepository.Object);
    }

    [Fact]
    public async Task Handle_DadosValidos_DeveChamarSave()
    {
        _fundRepository.Setup(r => r.CheckIfExistsByCode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundRepository.Setup(r => r.CheckIfExistsByDocument(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundTypeRepository.Setup(r => r.CheckIfExistsById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "11222333000181", FundType = 1 };

        await _handler.Handle(command, CancellationToken.None);

        _fundRepository.Verify(r => r.Save(It.IsAny<Fund>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CodigoJaExistente_DeveLancarConflictException()
    {
        _fundRepository.Setup(r => r.CheckIfExistsByCode("ITAU001", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "11222333000181", FundType = 1 };

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Handle_CnpjJaExistente_DeveLancarConflictException()
    {
        _fundRepository.Setup(r => r.CheckIfExistsByCode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundRepository.Setup(r => r.CheckIfExistsByDocument(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "11222333000181", FundType = 1 };

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Handle_TipoFundoInvalido_DeveLancarValidationException()
    {
        _fundRepository.Setup(r => r.CheckIfExistsByCode(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundRepository.Setup(r => r.CheckIfExistsByDocument(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundTypeRepository.Setup(r => r.CheckIfExistsById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateFundCommand { Code = "ITAU001", Name = "Fundo Teste", Document = "11222333000181", FundType = 99 };

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }
}
