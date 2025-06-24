using Microsoft.EntityFrameworkCore.Infrastructure;
using MySocialMedia.Models.Repositories;

namespace MySocialMedia.Models.UoW;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _appContext;

    private Dictionary<Type, object> _repositories;

    public UnitOfWork(ApplicationDbContext app)
    {
        _appContext = app;
    }

    public void Dispose()
    {
        _appContext?.Dispose();
        _repositories?.Clear();
    }

    public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class
    {
        if (_repositories == null)
        {
            _repositories = new Dictionary<Type, object>();
        }

        if (hasCustomRepository)
        {
            var customRepo = _appContext.GetService<IRepository<TEntity>>();
            if (customRepo != null)
            {
                return customRepo;
            }
        }

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new Repository<TEntity>(_appContext);
        }

        return (IRepository<TEntity>)_repositories[type];

    }
    public async Task<int> SaveChanges(bool ensureAutoHistory = false)
    {
        return await _appContext.SaveChangesAsync(ensureAutoHistory);
    }
}
