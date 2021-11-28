using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // Property for filtering by ProductId,ProductTypeId,ProductBrandId
         Expression<Func<T,bool>> Criteria {get;}


        // Property for including the Product Types and Brands.
         List<Expression<Func<T,object>>> Includes {get;}



        // Properties for Sorting
        Expression<Func<T,object>> OrderBy { get; }

        Expression<Func<T, object>> OrderByDescending { get; }


        //Properties for Pagination
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }

    }
}