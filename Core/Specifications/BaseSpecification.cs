using System.Linq.Expressions;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {        
        }
        
        public BaseSpecification(Expression<Func<T,bool>> criteria)
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

        protected void AddInclude(Expression<Func<T,Object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression; // Here you are able to set OrderBy coz you have set private
        }

        protected void AddOrderByDescending(Expression<Func<T,Object>> orderbyDesExpression)
        {
            OrderByDescending = orderbyDesExpression;
        }

        protected void ApplyPaging(int skip,int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}