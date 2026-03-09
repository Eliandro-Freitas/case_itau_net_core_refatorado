namespace CaseItau.Domain.Interfaces.Repositories;

public interface IFundTypeRepository
{
    Task<bool> CheckIfExistsById(int id, CancellationToken ct);
}