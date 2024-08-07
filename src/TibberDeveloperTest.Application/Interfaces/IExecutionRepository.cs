using TibberDeveloperTest.Domain.Entities;

namespace TibberDeveloperTest.Application.Interfaces;

public interface IExecutionsRepository
{
    Task<Execution> AddAsync(Execution execution, CancellationToken cancellationToken);
}