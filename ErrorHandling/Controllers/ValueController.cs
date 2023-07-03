using Microsoft.AspNetCore.Mvc;

namespace ErrorHandling.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ValueController : ControllerBase
	{
		[HttpGet("/get")]
		public string Get()
		{
			return "value get";
		}

		[HttpDelete("/delete")]
		public string Delete()
		{
			return "value delete";
		}

		[HttpDelete("/post")]
		public string Post()
		{
			return "value post";
		}

		[HttpDelete("/put")]
		public string Put()
		{
			return "value put";
		}
	}
}
