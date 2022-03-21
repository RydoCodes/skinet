using System.Linq.Expressions;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification()
        {        
        }
        
        protected BaseSpecification(Expression<Func<T,bool>> criteria)
        {
            Criteria = criteria; // You can set a property in constructor even without specifying private set for Criteria.
        }

        public Expression<Func<T, bool>> Criteria {get;}

        public List<Expression<Func<T, object>>> Includes {get;} = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; } // private set meaning setting the ability to set OrderBy only inside this class.

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        // can be called directly in the class which inherit BaseSpecification<T> class
        protected void AddInclude(Expression<Func<T,Object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // can be called directly in the class which inherit BaseSpecification<T> class
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression; // Here you are able to set OrderBy coz you have private set on it
        }

        // can be called directly in the class which inherit BaseSpecification<T> class
        protected void AddOrderByDescending(Expression<Func<T,Object>> orderbyDesExpression)
        {
            OrderByDescending = orderbyDesExpression; // Here you are able to set OrderByDescending coz you have private set on it
        }

        // can be called directly in the class which inherit BaseSpecification<T> class
        protected void ApplyPaging(int skip,int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}