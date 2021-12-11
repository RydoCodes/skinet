using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
	// This specification is just to return the count of all products which will be the part of output response Pagination<T> Class.
	public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
	{
		public ProductWithFiltersForCountSpecification(ProductSpecParams productParams) 
			: base(x =>
				(string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
				(!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
				(!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
			)
		{

		}
	}
}
