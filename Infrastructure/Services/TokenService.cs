using Core.Entities.Identity;
using Core.Interfaces.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _config;
		private readonly SymmetricSecurityKey _key;

		public TokenService(IConfiguration config)
		{
			this._config = config;
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"])); // ["Token:Key"]  //"Key": "super secret key",
			//SymmetricSecurityKey needs bytes[]
			//Symmetric Encryption means that there is no public and private key like there is in SSL.
			//Same key is used to encrypt and decrypt the signature here.
			//We would need to create a symmetric security key so that we can sign the token.
			
		}

		public string CreateToken(AppUser user) // Token is created for an user.
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email,user.Email),
				new Claim(ClaimTypes.GivenName, user.DisplayName)
			};

			// This algorithm will encrypt the _key using hmacsha512 Algorithm -> ["Token:Key"] and then Sign the token
			// HmacSha512s Encryption is done on the Symmetric key _key
			// Creds is a HmacSha512s encrypted signed in key
			SigningCredentials creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // Microsoft.IdentityModel.Tokens

			//Our token will ony be valid if it is before the expiry date and it was issued by Issuer.
			//SecurityTokenDescriptor : Contains some information which used to create a security token.

			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(1),
				SigningCredentials = creds,
				Issuer = _config["Token:Issuer"], // "https://localhost:5001"
				Audience = null
			};

			//JwtSecurityTokenHandler() handles our token
			JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler(); // System.IdentityModel.Tokens.Jwt

			// JwtSecurityTokenHandler.CreateToken() creates a JSON Web Token
			SecurityToken token = tokenhandler.CreateToken(tokenDescriptor);

			// Serializes a System.IdentityModel.Tokens.Jwt.JwtSecurityToken into a JWT in Compact Serialization Format.
			return tokenhandler.WriteToken(token);
		}
	}
}
