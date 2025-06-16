using MySocialMedia.Models.Repositoriesж;

namespace MySocialMedia.Models.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
                _db = db;
        }
       
        public void Create(T item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Get(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(T item)
        {
            throw new NotImplementedException(); // шаг 4й логика обновления 
        }
    }
}
