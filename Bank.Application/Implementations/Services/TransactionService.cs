
using Bank.Application.Dtos;
using Bank.Application.Interfaces;
using Bank.Application.Interfaces.Services;
using Bank.Domain.Entities;

using Dapper;

namespace Bank.Application.Implementations.Services;

public class TransactionService : ITransactionService
{
	private readonly IDatabase _db;
	public TransactionService(IDatabase db)
	{
		_db = db;
	}

	public async Task<bool> CreateTransactionAsync(TransactionCreateDto dto)
	{
		if(dto.Amount <= 0)
			throw new Exception("Amount must be greater than 0");
		string fromAccQuery = "SELECT * FROM accounts WHERE id = @id";
		string toAccQuery = "SELECT COUNT(*) FROM accounts WHERE id = @id";
		string decQuery = "UPDATE accounts SET balance = balance - @amount WHERE id = @id";
		string incQuery = "UPDATE accounts SET balance = balance + @amount WHERE id = @id";
		string transQuery = "INSERT INTO transactions (amount,fromAccountId,status,performedAt,toAccountId) VALUES (@amount,@fromAccountId,@status,@performedAt,@toAccountId)";
		Account? acc;
		using(var conn = _db.CreateConnection())
		{
			bool success = true;
			acc = await conn.QueryFirstOrDefaultAsync<Account>(fromAccQuery, new { id = dto.FromAccountId });
			if(acc is null)
				throw new Exception("Account not found");
			if(await conn.ExecuteScalarAsync<int>(toAccQuery, new { id = dto.ToAccountId }) < 1)
				throw new Exception("Account not found");

			if(acc.Balance < dto.Amount)
				success = false;

			Transaction entity = new()
			{
				Amount = dto.Amount,
				FromAccountId = dto.FromAccountId,
				Status = success ? "Success" : "Failed",
				PerformedAt = DateTime.Now,
				ToAccountId = dto.ToAccountId,
			};
			if(success)
			{
				await conn.ExecuteAsync(decQuery, new { id = dto.FromAccountId, amount = dto.Amount });
				await conn.ExecuteAsync(incQuery, new { id = dto.ToAccountId, amount = dto.Amount });
			}
			return await conn.ExecuteAsync(transQuery, entity) > 0;
		}
	}

	public async Task<decimal> GetProfitAsync(DateTime from, DateTime to)
	{
		string query = "SELECT SUM(amount)*0.01 FROM transactions WHERE performedAt BETWEEN @from AND @to AND status = 'Success'";
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			return await conn.ExecuteScalarAsync<decimal>(query, new { from, to });
		}
	}

	public async Task<GetManyDto<Transaction>> GetSuspiciousTransactionsAsync(decimal maxAmount = 10000, int maxTransactions = 5, int page = 1, int pageSize = 50)
	{
		if(pageSize >= 100)
			pageSize = 100;
		string totalQuery = """
			select
				count(*)
			from transactions as t
			join accounts as a on t.fromAccountId = a.id
			where
				(t.amount>=@maxAmount) or
				(a.customerId in
					(select
						c.id
					from transactions as t
					join accounts as a on t.fromAccountId = a.id
					join customers as c on a.customerId = c.id
					where t.performedAt > current_timestamp - interval '10 minutes'
					group by c.id
					having count(*) > @maxTransactions) and
				t.performedAt > current_timestamp - interval '10 minutes')
			""";
		string query = """
			select
				t.*
			from transactions as t
			join accounts as a on t.fromAccountId = a.id
			where
				(t.amount>=@maxAmount) or
				(a.customerId in
					(select
						c.id
					from transactions as t
					join accounts as a on t.fromAccountId = a.id
					join customers as c on a.customerId = c.id
					where t.performedAt > current_timestamp - interval '10 minutes'
					group by c.id
					having count(*) > @maxTransactions) and
				t.performedAt > current_timestamp - interval '10 minutes')
			order by t.performedAt desc
			""" + $" limit {pageSize} offset {(page - 1) * pageSize}";
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			int total = await conn.ExecuteScalarAsync<int>(totalQuery, new { maxAmount, maxTransactions });
			return new GetManyDto<Transaction>(await conn.QueryAsync<Transaction>(query, new { maxAmount, maxTransactions }), total);
		}
	}

	public async Task<IEnumerable<CustomerGetDto>> GetTopCustomersAsync(int count = 5)
	{
		if(count >= 50)
			count = 50;
		string query = """
			select
				c.id,
				c.name,
				sum(amount) as turnover
			from transactions as t
			join accounts as a on t.fromAccountId = a.id
			join accounts as a1 on t.toAccountId = a1.id
			join customers as c on a.customerId = c.id or a1.customerId = c.id
			where t.status='Success'
			group by c.id,c.name
			order by sum(amount) desc
			""" + $" limit {count}";
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			return await conn.QueryAsync<CustomerGetDto>(query);
		}
	}

	public async Task<GetManyDto<Transaction>> GetTransactionsFilteredAsync(int? customerId = null, DateTime? dateFrom = null, DateTime? dateTo = null, decimal? minAmount = null, decimal? maxAmount = null, string? status = null, int page = 1, int pageSize = 50)
	{
		if(pageSize >= 100)
			pageSize = 100;
		dateFrom ??= DateTime.MinValue;
		dateTo ??= DateTime.MaxValue;
		string totalQuery = """
			select
				count(*)
			from transactions as t
			join accounts as a on t.fromAccountId = a.id
			where
				(@customerId is null or a.customerId = @customerId) and
				(@dateFrom is null or t.performedAt >= @dateFrom) and
				(@dateTo is null or t.performedAt <= @dateTo) and
				(@minAmount is null or t.amount >= @minAmount) and
				(@maxAmount is null or t.amount <= @maxAmount) and
				(@status is null or t.status = @status)
			""";
		string query = """
			select
				t.*
			from transactions as t
			join accounts as a on t.fromAccountId = a.id
			where
				(@customerId is null or a.customerId = @customerId) and
				(@dateFrom is null or t.performedAt >= @dateFrom) and
				(@dateTo is null or t.performedAt <= @dateTo) and
				(@minAmount is null or t.amount >= @minAmount) and
				(@maxAmount is null or t.amount <= @maxAmount) and
				(@status is null or t.status = @status)
			""" + $" limit {pageSize} offset {(page - 1) * pageSize}";
		using(var conn = _db.CreateConnection())
		{
			conn.Open();
			return new GetManyDto<Transaction>(
				 await conn.QueryAsync<Transaction>(query, new { customerId, dateFrom, dateTo, minAmount, maxAmount, status }), await conn.ExecuteScalarAsync<int>(totalQuery, new { customerId, dateFrom, dateTo, minAmount, maxAmount, status }));
		}
	}
}

