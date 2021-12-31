using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.IdentityDTOs
{
	// The Error message returned by Required Attribute for AddressDto is overriden agains the defalt one. 
	public class AddressDto
	{
		[Required(ErrorMessage ="First Name chahiye mujhe")]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Street { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string State { get; set; }
		[Required]	
		public string ZipCode { get; set; }
	}
}
