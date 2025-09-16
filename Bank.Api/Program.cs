using Bank.Application.Implementations.Services;
using Bank.Application.Interfaces;
using Bank.Application.Interfaces.Services;
using Bank.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDatabase, PostgresDatabase>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ExceptionHandlerMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
	app.UseSwaggerUI();
	app.MapOpenApi("swagger/v1/swagger.json");
}

app.UseHttpsRedirection();

app.UseAuthorization();



app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
