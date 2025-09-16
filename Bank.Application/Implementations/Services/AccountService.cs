
using Bank.Application.Interfaces;
using Bank.Application.Interfaces.Services;
using Bank.Domain.Entities;

using Dapper;

namespace Bank.Application.Implementations.Services;

public class AccountService : IAccountService
{
	private readonly IDatabase _db;

	public AccountService(IDatabase db)
	{
		_db = db;
	}

	public async Task<bool> Create(Account entity)
	{
		string query = "INSERT INTO accounts (customerId, number,balance,currency) VALUES (@customerId,@number,@balance,@currency)";
		entity.Number = Guid.NewGuid().ToString()[..16];
		using var conn = _db.CreateConnection();
		conn.Open();
		return await conn.ExecuteAsync(query, entity) > 0;
	}

	public async Task<bool> Delete(int id)
	{
		string query = "DELETE FROM accounts WHERE id = @id";
		using var conn = _db.CreateConnection();
		conn.Open();
		return await conn.ExecuteAsync(query, new { id }) > 0;
	}

	public async Task<Account> Get(int id)
	{
		string query = "SELECT * FROM accounts WHERE id = @id";
		using var conn = _db.CreateConnection();
		conn.Open();
		return await conn.QueryFirstOrDefaultAsync<Account>(query, new { id }) ?? throw new Exception("Account not found");
	}

	public Task<IEnumerable<Account>> GetAll()
	{
		string query = "SELECT * FROM accounts";
		using var conn = _db.CreateConnection();
		conn.Open();
		return conn.QueryAsync<Account>(query);
	}

	public async Task<bool> Update(Account entity)
	{
		string query = "UPDATE accounts SET customerId = @customerId, number = @number, balance = @balance, currency = @currency WHERE id = @id";
		using var conn = _db.CreateConnection();
		conn.Open();
		return await conn.ExecuteAsync(query, entity) > 0;
	}
}

