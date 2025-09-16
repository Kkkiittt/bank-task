using System.Data.Common;

namespace Bank.Application.Interfaces;

public interface IDatabase
{
	public DbConnection CreateConnection();
}
