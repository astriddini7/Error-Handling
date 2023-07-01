using ErrorHandling.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Get Config
var configuration = new ConfigurationBuilder()
	.SetBasePath(builder.Environment.ContentRootPath)
	.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.Build();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>(configuration.GetConnectionString("DefaultConnection"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
