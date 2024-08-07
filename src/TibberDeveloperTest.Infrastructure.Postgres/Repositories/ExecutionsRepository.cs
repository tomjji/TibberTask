using TibberDeveloperTest.Application.Interfaces;
using TibberDeveloperTest.Domain.Entities;
using TibberDeveloperTest.Infrastructure.Postgres.Contexts;

namespace TibberDeveloperTest.Infrastructure.Postgres.Repositories;

public class ExecutionsRepository(ApplicationDbContext dbContext) : IExecutionsRepository
{
    public async Task<Execution> AddAsync(Execution execution, CancellationToken cancellationToken)
    {
        var row = await dbContext.Executions.AddAsync(execution, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return row.Entity;
    }
}