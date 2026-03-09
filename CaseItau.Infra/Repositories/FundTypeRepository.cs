using CaseItau.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Repositories;

public class FundTypeRepository(DatabaseContext context) : IFundTypeRepository
{
    private readonly DatabaseContext _context = context;

    public async Task<bool> CheckIfExistsById(int id, CancellationToken ct)
        => await _context.FundTypes.AnyAsync(x => x.Id == id, ct);
}