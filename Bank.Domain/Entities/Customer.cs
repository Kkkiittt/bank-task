namespace Bank.Domain.Entities;

public class Customer
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTime RegisteredAt { get; set; }
	public bool Active { get; set; } = true;
}

