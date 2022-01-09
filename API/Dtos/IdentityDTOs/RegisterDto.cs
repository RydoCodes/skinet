using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.IdentityDTOs
{
	public class RegisterDto
	{
		[Required(ErrorMessage = "Display Name jaruri hai. Set at RegisterDTO")]
		public string DisplayName { get; set; }

		[Required(ErrorMessage = "Email galat hai and format me nahi hai. Set at RegisterDTO")]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",ErrorMessage ="Sahi Password Dal Rydo")]
		public string Password { get; set; }
	}
}
