using MySocialMedia.Models.Repositories;

namespace MySocialMedia.Models.UoW
{
    /// <summary>
    /// хранилище всех репозиториев проекта.  UnitOfWork паттерн
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        //сохранение всех изменений в базу данных (по всем репозиториям).
        int SaveChanges(bool ensureAutoHistory = false);

        //это все репозитории для  самописных моделей.
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
