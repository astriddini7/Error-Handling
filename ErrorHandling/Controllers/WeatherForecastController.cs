using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace ErrorHandling.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			string projectDirectory = Directory.GetCurrentDirectory();
			string scriptPath = Path.Combine(projectDirectory, "Sql\\StoreProcedur\\SP_LogErrors.sql");

			string script = System.IO.File.ReadAllText(scriptPath);

			string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=error_handling;";

			// Membuat objek koneksi
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				// Membuat objek command untuk stored procedure
				using (SqlCommand command = new SqlCommand(script, connection))
				{
					command.CommandType = CommandType.Text;

					// Menambahkan parameter ke stored procedure
					command.Parameters.AddWithValue("@Url", "/GetWeatherForecast");

					// Eksekusi stored procedure
					command.ExecuteNonQuery();

					using (SqlDataReader reader = command.ExecuteReader())
					{
						var data = reader.Read();
						var astrid = "";
						//while (reader.Read())
						//{
						//	// Access the values from the current row
						//	int id = reader.GetInt32(0);
						//	string name = reader.GetString(1);
						//	DateTime date = reader.GetDateTime(2);

						//	// Process the retrieved data as needed
						//	Console.WriteLine($"ID: {id}, Name: {name}, Date: {date}");
						//}
					}
				}

				//throw new Exception("Waduh system error nih");

				return Enumerable.Range(1, 5).Select(index => new WeatherForecast
				{
					Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					TemperatureC = Random.Shared.Next(-20, 55),
					Summary = Summaries[Random.Shared.Next(Summaries.Length)]
				})
				.ToArray();
			}
		}

		//[HttpGet(Name = "GetError")]
		//public IEnumerable<Erros> GetError()
		//{
		//	var errors = new List<Erros>();

		//	string projectDirectory = Directory.GetCurrentDirectory();
		//	string scriptPath = Path.Combine(projectDirectory, "Sql\\StoreProcedur\\SP_LogErrors.sql");


		//	string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=error_handling;";

		//	// Membuat objek koneksi
		//	using (SqlConnection connection = new SqlConnection(connectionString))
		//	{
		//		connection.Open();

		//		// Membuat objek command untuk stored procedure
		//		using (SqlCommand command = new SqlCommand("NamaStoredProsedur", connection))
		//		{
		//			command.CommandType = CommandType.StoredProcedure;

		//			// Menambahkan parameter ke stored procedure
		//			command.Parameters.AddWithValue("@Parameter1", "Nilai1");
		//			command.Parameters.AddWithValue("@Parameter2", "Nilai2");

		//			// Eksekusi stored procedure
		//			command.ExecuteNonQuery();
		//		}
		//	}

		//	return errors;
		//}

		public class Erros
		{
			public string Url { get; set; }
		}
	}
}