using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Fund> Funds { get; set; }
    public DbSet<FundType> FundTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
}