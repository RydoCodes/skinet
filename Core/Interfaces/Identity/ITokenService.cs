using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Identity
{
	public interface ITokenService
	{
		string CreateToken(AppUser user);
	}
}
