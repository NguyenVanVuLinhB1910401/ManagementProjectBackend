namespace ManagementProject.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(string id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
