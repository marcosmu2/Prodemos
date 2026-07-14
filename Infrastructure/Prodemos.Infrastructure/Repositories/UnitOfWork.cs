using Prodemos.Application.Persistence;
using Prodemos.Infrastructure.Persistence;
using System.Collections;

namespace Prodemos.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private Hashtable? _repositories;
    private readonly ProdemosDbContext _context;

    public UnitOfWork(ProdemosDbContext prodemosDbContext)
    {
        _context = prodemosDbContext;
    }

    public async Task<int> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error in transaction", ex);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories is null) {
            _repositories = new Hashtable();
        }

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IAsyncRepository<TEntity>)_repositories[type]!;
    }
}
