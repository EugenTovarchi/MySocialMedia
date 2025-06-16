namespace MySocialMedia.Models.Repositories;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    Task<T> Get(int id);
    void Create(T item);
    void Update(T item);  
    void Delete(int id);
}
