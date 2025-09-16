using Bank.Application.Interfaces.Services;
using Bank.Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
	private readonly IAccountService _serv;

	public AccountController(IAccountService serv)
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
	public async Task<IActionResult> Post([FromBody] Account account)
	{
		return Ok(await _serv.Create(account));
	}

	[HttpPut]
	public async Task<IActionResult> Put([FromBody] Account account)
	{
		return Ok(await _serv.Update(account));
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		return Ok(await _serv.Delete(id));
	}
}
