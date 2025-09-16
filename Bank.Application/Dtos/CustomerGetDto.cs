namespace Bank.Application.Dtos;

public class CustomerGetDto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Turnover { get; set; }
}

