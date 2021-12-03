using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{

	//app.UseMiddleware<ExceptionMiddleware>(); : handle null reference exception
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context) 
		{
			try
			{
				await _next(context);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var response = _env.IsDevelopment()
								? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) // we are giving more details when in DEV Mode
								: new ApiException((int)HttpStatusCode.InternalServerError); // we are giving only status code when in PROD Mode.

				var options = new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};

				var json = JsonSerializer.Serialize(response, options);

				await context.Response.WriteAsync(json);
			}
		}
	}
}
