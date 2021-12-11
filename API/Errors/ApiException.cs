using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
	// This will return Status Code, Message from the base class (ApiResponse) and details from the present class (ApiException)
	public class ApiException : ApiResponse
	{
		public ApiException(int statusCode, string message = null,string details=null) : base(statusCode, message)
		{
			Details = details;
		}

		public string Details { get; set; }
	}
}
