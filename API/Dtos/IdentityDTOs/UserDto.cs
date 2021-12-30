using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.IdentityDTOs.Dtos
{
	public class UserDto
	{
		public string Email { get; set; }
		public string DisplayName { get; set; } // We will display this in our navbar in our angular app
		public string Token { get; set; }
	}
}
