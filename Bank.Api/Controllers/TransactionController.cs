using Bank.Application.Dtos;
using Bank.Application.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;


[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
	private readonly ITransactionService _serv;

	public TransactionController(ITransactionService serv)
	{
		_serv = serv;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] TransactionCreateDto transaction)
	{
		return Ok(await _serv.CreateTransactionAsync(transaction));
	}

	[HttpGet]
	public async Task<IActionResult> GetFilteredAsync(int? customerId = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		decimal? minAmount = null,
		decimal? maxAmount = null,
		string? status = null,
		int page = 1,
		int pageSize = 50)
	{
		return Ok(await _serv.GetTransactionsFilteredAsync(customerId, dateFrom, dateTo, minAmount, maxAmount, status, page, pageSize));
	}

	[HttpGet("customers/top/{count}")]
	public async Task<IActionResult> GetTopCustomersAsync(int count = 5)
	{
		return Ok(await _serv.GetTopCustomersAsync(count));
	}

	[HttpGet("profit")]
	public async Task<IActionResult> GetProfitAsync([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
	{
		return Ok(await _serv.GetProfitAsync(dateFrom, dateTo));
	}

	[HttpGet("sus")]
	public async Task<IActionResult> GetSusAsync(decimal maxAmount = 10000, int maxTransactions = 5, int page = 1, int pageSize = 50)
	{
		return Ok(await _serv.GetSuspiciousTransactionsAsync(maxAmount, maxTransactions, page, pageSize));
	}
}
