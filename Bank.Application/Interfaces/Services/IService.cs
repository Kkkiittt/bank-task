namespace Bank.Application.Interfaces.Services;

public interface IService<T> where T : class
{
	public Task<T> Get(int id);
	public Task<bool> Delete(int id);
	public Task<IEnumerable<T>> GetAll();
	public Task<bool> Update(T entity);
	public Task<bool> Create(T entity);
}
