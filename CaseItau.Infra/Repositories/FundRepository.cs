using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Repositories;

public class FundRepository(DatabaseContext context) : IFundRepository
{
    private readonly DatabaseContext _context = context;

    public async Task<bool> CheckIfExistsByCode(string code, CancellationToken ct)
        => await _context.Funds.AnyAsync(x => x.Code == code, ct);

    public async Task<bool> CheckIfExistsByDocument(string document, CancellationToken cancellationToken)
        => await _context.Funds.AnyAsync(f => f.Document.Value == document, cancellationToken);

    public async Task Save(Fund fund, CancellationToken ct)
    {
        await _context.AddAsync(fund, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Delete(Fund fund, CancellationToken ct)
    {
        _context.Remove(fund);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Fund>> Get(CancellationToken ct) 
        => await _context
            .Funds
            .Include(x => x.FundType)
            .ToListAsync(ct);

    public async Task<Fund> GetByCode(string code, CancellationToken ct)
        => await _context
            .Funds
            .Include(x => x.FundType)
            .FirstOrDefaultAsync(x => x.Code == code, ct);

    public async Task Update(Fund fund, CancellationToken ct)
    {
        _context.Update(fund);
        await _context.Entry(fund).Reference(f => f.FundType).LoadAsync(ct);
        await _context.SaveChangesAsync(ct);
    }
}