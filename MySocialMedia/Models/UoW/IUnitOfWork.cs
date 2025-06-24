using MySocialMedia.Models.Repositories;

namespace MySocialMedia.Models.UoW
{
    /// <summary>
    /// хранилище всех репозиториев проекта.  UnitOfWork паттерн
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        //сохранение всех изменений в базу данных (по всем репозиториям).
         Task<int> SaveChanges(bool ensureAutoHistory = false);
         
        //это все репозитории для  самописных моделей. Мы не делаем тут метод асинхронным т.к. это паттер фабрика и тут это не нужно 
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
