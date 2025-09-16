using System.Data.Common;

using Bank.Application.Interfaces;

using Npgsql;

namespace Bank.Infrastructure;

public class PostgresDatabase : IDatabase
{
	private readonly string _connectionString;

	public PostgresDatabase(string connectionString="host=localhost;port=5432;database=BankDb;user id=postgres;password=somepass")
	{
		_connectionString = connectionString;
	}

	public DbConnection CreateConnection()
	{
		return new NpgsqlConnection(_connectionString);
	}
}

