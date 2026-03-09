using CaseItau.Domain.Interfaces.Repositories;
using CaseItau.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaseItau.Infrastructure.DependenceInjections;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var connection = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlite(connection);
        });

        services.AddScoped<IFundRepository, FundRepository>();
        services.AddScoped<IFundTypeRepository, FundTypeRepository>();

        return services;
    }
}