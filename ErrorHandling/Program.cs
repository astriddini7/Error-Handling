using ErrorHandling.Middlewares;
using Microsoft.AspNetCore.HttpLogging;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
	// Customize HTTP logging here.
	logging.LoggingFields = HttpLoggingFields.All;
	logging.RequestHeaders.Add("My-Request-Header");
	logging.ResponseHeaders.Add("My-Response-Header");
	logging.MediaTypeOptions.AddText("application/javascript");
	logging.RequestBodyLogLimit = 4096;
	logging.ResponseBodyLogLimit = 4096;
});

// Sample Result
// info: Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware[1]
// 	  Request:
// Protocol: HTTP / 1.1
// 	  Method: GET
// 	  Scheme: https
// 	  PathBase:
//       Path: /
// 	  QueryString:
//       Connection: keep - alive
// 	  Accept: */*
//       Accept-Encoding: gzip, deflate, br
//       Host: localhost:5001
//       User-Agent: PostmanRuntime/7.26.5
//       My-Request-Header: blogpost-sample
//       Postman-Token: [Redacted]

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

app.UseMiddleware<AccessMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHttpLogging();

app.MapControllers();

app.Run();
