using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
	public static class UserManagerExtensions
	{
		public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> input, ClaimsPrincipal user)
		{
			Claim emalclaim = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email);
			string email = emalclaim.Value;

			return await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
			// Users is of type IQueryable<AppUser>
		}

		public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> input, ClaimsPrincipal user)
		{
			Claim emalclaim = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email);
			string email = emalclaim.Value;

			AppUser loggedinuser = await input.Users.SingleOrDefaultAsync(x => x.Email == email);

			return loggedinuser;
		}
	}
}
