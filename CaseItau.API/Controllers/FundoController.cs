using CaseItau.Application.Commands.CreateFund;
using CaseItau.Application.Commands.DeleteFund;
using CaseItau.Application.Commands.UpdateFund;
using CaseItau.Application.DTOs;
using CaseItau.Application.Features.Funds.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseItau.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FundoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Listar todos os fundos
    /// </summary>
    /// <remarks>
    /// Retorna todos os fundos cadastrados com tipo específico e patrimônio.
    ///
    /// Exemplo de retorno:
    ///
    ///     [
    ///       {
    ///         "code": "ITAUTESTE01",
    ///         "name": "Fundo Teste",
    ///         "document": "12345678000190",
    ///         "grossValue": 1500000.00,
    ///         "fundType": {
    ///           "id": 1,
    ///           "name": "RENDA_FIXA"
    ///         }
    ///       }
    ///     ]
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FundResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<FundResult>>> Get()
    {
        var query = new GetFundsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Buscar fundo pelo código
    /// </summary>
    /// <remarks>
    /// Retorna um fundo específico a partir do seu código.
    ///
    /// Exemplo de retorno:
    ///
    ///     {
    ///       "code": "ITAUTESTE01",
    ///       "name": "Fundo Teste",
    ///       "document": "12345678000190",
    ///       "grossValue": 1500000.00,
    ///       "fundType": {
    ///         "id": 1,
    ///         "name": "RENDA_FIXA"
    ///       }
    ///     }
    ///
    /// Exemplo de requisição:
    ///
    ///     GET /api/Fundo/ITAUTESTE01
    ///
    /// </remarks>
    /// <param name="code">Código do fundo a ser consultado</param>
    /// <response code="200">Retorna o fundo encontrado</response>
    /// <response code="404">Fundo não encontrado</response>
    /// <response code="500">Erro interno no servidor</response>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(FundResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FundResult>> GetByCode(string code)
    {
        var query = new GetFundByCodeQuery
        {
            Code = code
        };

        var fund = await _mediator.Send(query);
        return Ok(fund);
    }

    /// <summary>
    /// Criar um novo fundo
    /// </summary>
    /// <remarks>
    /// Cria um novo fundo de investimento.
    ///
    /// Tipos de fundo disponíveis:
    ///
    ///     1 - RENDA FIXA
    ///     2 - ACOES
    ///     3 - MULTI MERCADO
    ///
    /// Exemplo de requisição:
    ///
    ///     POST /api/Fundo
    ///     {
    ///       "code": "ITAUTESTE01",
    ///       "name": "Fundo Teste",
    ///       "document": "12345678000190",
    ///       "fundType": 1,
    ///       "grossValue": 1500000.00
    ///     }
    ///
    /// </remarks>
    /// <param name="command">Dados do fundo a ser criado</param>
    /// <response code="201">Fundo criado com sucesso</response>
    /// <response code="400">Dados inválidos ou fundo já existente</response>
    /// <response code="500">Erro interno no servidor</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post([FromBody] CreateFundCommand command)
    {
        await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, null);
    }

    /// <summary>
    /// Atualizar um fundo existente
    /// </summary>
    /// <remarks>
    /// Atualiza os dados de um fundo existente.
    ///
    /// Tipos de fundo disponíveis:
    ///
    ///     1 - RENDA FIXA
    ///     2 - ACOES
    ///     3 - MULTI MERCADO
    ///
    /// Exemplo de requisição:
    ///
    ///     PUT /api/Fundo/ITAUTESTE01
    ///     {
    ///       "code": "ITAUTESTE01",
    ///       "name": "Novo Nome",
    ///       "document": "12345678000190",
    ///       "fundType": 2
    ///     }
    ///
    /// </remarks>
    /// <param name="code">Código do fundo a ser atualizado</param>
    /// <param name="command">Dados atualizados do fundo</param>
    /// <response code="200">Fundo atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Fundo não encontrado</response>
    /// <response code="409">Já existe um fundo com o código ou CNPJ informado</response>
    /// <response code="500">Erro interno no servidor</response>
    [HttpPut("{code}")]
    [ProducesResponseType(typeof(FundResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FundResult>> Put(string code, [FromBody] UpdateFundCommand command)
    {
        command.Code = code;
        var fundResult = await _mediator.Send(command);
        return Ok(fundResult);
    }

    /// <summary>
    /// Remover um fundo existente
    /// </summary>
    /// <remarks>
    /// Remove permanentemente um fundo de investimento a partir do seu código.
    ///
    /// Exemplo de requisição:
    ///
    ///     DELETE /api/Fundo/ITAUTESTE01
    ///
    /// </remarks>
    /// <param name="code">Código do fundo a ser removido</param>
    /// <response code="204">Fundo removido com sucesso</response>
    /// <response code="404">Fundo não encontrado</response>
    /// <response code="500">Erro interno no servidor</response>
    [HttpDelete("{code}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(string code)
    {
        await _mediator.Send(new DeleteFundByCodeCommand() { Code = code });
        return NoContent();
    }

    /// <summary>
    /// Atualizar o patrimônio de um fundo
    /// </summary>
    /// <remarks>
    /// Adiciona ou subtrai um valor do patrimônio bruto de um fundo existente.
    ///
    /// O valor informado é somado ao patrimônio atual. Use valores negativos para subtrair.
    ///
    /// Exemplos de requisição:
    ///
    ///     PUT /api/Fundo/ITAUTESTE01/patrimonio
    ///     { "grossValue": 500.00 }   → adiciona 500 ao patrimônio atual
    ///
    ///     PUT /api/Fundo/ITAUTESTE01/patrimonio
    ///     { "grossValue": -200.00 }  → subtrai 200 do patrimônio atual
    ///
    /// </remarks>
    /// <param name="code">Código do fundo a ser atualizado</param>
    /// <param name="command">Valor a adicionar (positivo) ou subtrair (negativo) do patrimônio</param>
    /// <response code="200">Patrimônio atualizado com sucesso</response>
    /// <response code="404">Fundo não encontrado</response>
    /// <response code="400">Valor de patrimônio inválido</response>
    /// <response code="500">Erro interno no servidor</response>
    [HttpPut("{code}/patrimonio")]
    [ProducesResponseType(typeof(FundResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FundResult>> PutGrossValue(string code, [FromBody] UpdateFundGrossValueCommand command)
    {
        command.Code = code;
        var fundResult = await _mediator.Send(command);
        return Ok(fundResult);
    }
}