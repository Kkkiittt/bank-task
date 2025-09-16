
using Bank.Application.Interfaces;
using Bank.Application.Interfaces.Services;
using Bank.Domain.Entities;

using Dapper;

namespace Bank.Application.Implementations.Services;

public class CustomerService : ICustomerService
{
	private readonly IDatabase _db;
	public CustomerService(IDatabase db)
	{
		_db = db;
	}

	public async Task<bool> Create(Customer entity)
	{
		string query = "INSERT INTO customers (name,phone,email,registeredat,active) VALUES (@name,@phone,@email,@registeredat,@active)";
		entity.Active = true;
		entity.RegisteredAt = DateTime.Now;
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			return await conn.ExecuteAsync(query, entity) > 0;
		}
	}

	public async Task<bool> Delete(int id)
	{
		string query = "DELETE FROM customers WHERE id = @id";
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			return await conn.ExecuteAsync(query, new { id }) > 0;
		}
	}

	public async Task<Customer> Get(int id)
	{
		string query = "SELECT * FROM customers WHERE id = @id";
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			return await conn.QueryFirstOrDefaultAsync<Customer>(query, new { id }) ?? throw new Exception("Customer not found");
		}
	}

	public async Task<IEnumerable<Customer>> GetAll()
	{
		string query = "SELECT * FROM customers";
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			return await conn.QueryAsync<Customer>(query);
		}
	}

	public async Task<bool> Update(Customer entity)
	{
		string query = "UPDATE customers SET name = @name, phone = @phone, email = @email,active=@active WHERE id = @id";
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			return await conn.ExecuteAsync(query, entity) > 0;
		}
	}
}

