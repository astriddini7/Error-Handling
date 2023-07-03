namespace ErrorHandling.Middlewares
{
	public class AccessMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<AccessMiddleware> _logger;

		public AccessMiddleware(RequestDelegate next, ILogger<AccessMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			string requestPath = context.Request.RouteValues.Where(a => a.Key == "controller").Select(a => a.Value).FirstOrDefault() + context.Request.Path;
			string requestMethod = context.Request.Method;
			string userName = context.User.Identity.Name;

			// Menentukan aturan akses berdasarkan routing dan nama pengguna
			if (requestPath.ToLower().StartsWith("value/post"))
			{
				// Jika Murni mencoba mengakses astrid/post dengan metode POST, kirim respon larangan (forbidden)
				throw new MethodAccessException("Access to this route is forbidden");
			}

			// Melanjutkan ke middleware berikutnya jika tidak ada aturan akses yang melarang
			await _next(context);
		}
	}
}
