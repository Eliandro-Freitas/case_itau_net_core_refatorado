using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Interfaces.Repositories;

public interface IFundRepository
{
    Task<IEnumerable<Fund>> Get(CancellationToken ct);
    Task<Fund> GetByCode(string code, CancellationToken ct);
    Task Update(Fund fund, CancellationToken ct);
    Task Delete(Fund fund, CancellationToken ct);
    Task Save(Fund fund, CancellationToken ct);
    Task<bool> CheckIfExistsByCode(string code, CancellationToken ct);
    Task<bool> CheckIfExistsByDocument(string document, CancellationToken ct);
}