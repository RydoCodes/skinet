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

	//app.UseMiddleware<ExceptionMiddleware>(); : handle null reference exception, 500 Status Code
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

		//  next is a function that can process an HTTP request.
		public async Task InvokeAsync(HttpContext context) 
		{
			try
			{
				// if there is no exception, we want the middleware to move on to the next piece of middleware.
				await _next(context);	
			}
			// if there is an exception, then we want to catch it.
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				ApiException apiexceptionresponse = _env.IsDevelopment()
								? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) // we are giving more details when in DEV Mode
								: new ApiException((int)HttpStatusCode.InternalServerError); // we are giving only status code when in PROD Mode.


				JsonSerializerOptions options = new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase // “StatusCode” is PascalCase. “statuscode” is camelcase
				};

				string json = JsonSerializer.Serialize(apiexceptionresponse, options);

				

				await context.Response.WriteAsync(json);
			}

			
		}
	}
}
