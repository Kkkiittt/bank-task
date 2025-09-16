public class ExceptionHandlerMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch(Exception ex)
		{
			context.Response.StatusCode = 400;
			context.Response.ContentType = "text/plain";
			await context.Response.WriteAsync(ex.Message);
		}
	}
}