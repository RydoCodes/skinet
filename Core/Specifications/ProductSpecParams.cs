using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
	public class ProductSpecParams
	{
		public const int MaxPageSize = 50;
		public int PageIndex { get; set; } = 1;

		public int? BrandId { get; set; }
		public int? TypeId { get; set; }

		public string Sort { get; set; }

		private int _pageSize =  6;

		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value>MaxPageSize)? MaxPageSize : value;
		}

		private string _search;

		//And what we want to make sure is that we're always checking against a lower case property here, 
		//even if the user types everything in capitals, we always wanted to match against something lowercase.
		public string Search
		{
			get => _search;
			set => _search = value.ToLower();
		}

		

	}
}
