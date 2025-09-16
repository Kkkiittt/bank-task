namespace Bank.Application.Dtos;

public class GetManyDto<T>
{
	public int Total { get; set; }
	public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

	public GetManyDto(IEnumerable<T> data, int total)
	{
		Data = data;
		Total = total;
	}
}

