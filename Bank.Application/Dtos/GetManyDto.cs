namespace Bank.Application.Dtos;

public class GetManyDto<T>(IEnumerable<T> data, int total)
{
	public int Total { get; set; } = total;
	public IEnumerable<T> Data { get; set; } = data;
}

