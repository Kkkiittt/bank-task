using Bank.Application.Interfaces.Services;
using Bank.Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
	private readonly ICustomerService _serv;

	public CustomerController(ICustomerService serv)
	{
		_serv = serv;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		return Ok(await _serv.GetAll());
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id)
	{
		return Ok(await _serv.Get(id));
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] Customer customer)
	{
		return Ok(await _serv.Create(customer));
	}

	[HttpPut]
	public async Task<IActionResult> Put([FromBody] Customer customer)
	{
		return Ok(await _serv.Update(customer));
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		return Ok(await _serv.Delete(id));
	}
}
