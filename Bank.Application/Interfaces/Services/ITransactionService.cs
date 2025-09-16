using Bank.Application.Dtos;
using Bank.Domain.Entities;

namespace Bank.Application.Interfaces.Services;

public interface ITransactionService
{
	public Task<bool> CreateTransactionAsync(TransactionCreateDto dto);
	public Task<GetManyDto<Transaction>> GetTransactionsFilteredAsync(
		int? customerId = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		decimal? minAmount = null,
		decimal? maxAmount = null,
		string? status = null,
		int page = 1,
		int pageSize = 50
		);
	public Task<IEnumerable<CustomerGetDto>> GetTopCustomersAsync(int count = 5);
	public Task<decimal> GetProfitAsync(DateTime from, DateTime to);
	public Task<GetManyDto<Transaction>> GetSuspiciousTransactionsAsync(decimal maxAmount = 10000, int maxTransactions = 5, int page=1, int pageSize=50);
}
