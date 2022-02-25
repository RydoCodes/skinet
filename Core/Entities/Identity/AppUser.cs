using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Identity
{
	// IdentityUser : An Identity class that we get from .NET Core Identity.
	public class AppUser : IdentityUser
	{
		public string DisplayName { get; set; }
		public Address Address { get; set; }
	}
}
