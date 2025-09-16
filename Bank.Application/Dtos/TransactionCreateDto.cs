namespace Bank.Application.Dtos;

public class TransactionCreateDto
{
	public int FromAccountId { get; set; }
	public int ToAccountId { get; set; }
	public decimal Amount { get; set; }
}

