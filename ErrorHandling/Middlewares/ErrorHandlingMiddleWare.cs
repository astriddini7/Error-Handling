using System.Data.SqlClient;

namespace ErrorHandling.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;
		private readonly string _connectionString;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, string connectionString)
		{
			_next = next;
			_logger = logger;
			_connectionString = connectionString;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				var statusCode = StatusCodes.Status500InternalServerError;
				var url = $"{context.Request.Path}{context.Request.QueryString}";

				_logger.LogError(ex, "An error occurred");

				if(ex is MethodAccessException)
					statusCode = StatusCodes.Status405MethodNotAllowed;
				else
					// Save error to SQL Server database
					SaveErrorToDatabase(ex, url);

				context.Response.StatusCode = statusCode;
				await context.Response.WriteAsync(ex.Message);
			}
		}

		private void SaveErrorToDatabase(Exception ex, string url)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = new SqlCommand("INSERT INTO LogError ([MessageError],[Url],[CreatedTime]) VALUES (@ErrorMessage, @Url, @CreatedTime)", connection))
				{
					command.Parameters.AddWithValue("@ErrorMessage", $"Error : {ex.Message}\n StackTrace : {ex.StackTrace}");
					command.Parameters.AddWithValue("@Url", url);
					command.Parameters.AddWithValue("@CreatedTime", DateTime.Now);
					command.ExecuteNonQuery();
				}
				connection.Close();
			}
		}
	}
}
