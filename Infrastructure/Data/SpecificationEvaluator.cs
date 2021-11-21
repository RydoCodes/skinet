
using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity: BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if(spec.Criteria!=null)
            {
                query = query.Where(spec.Criteria); // query : IQueryable<T> has a where method which expects a parameter of type Expression<Func<T,bool> and return  IQueryable<T>
                                                    // p=> p.ProductTypeId == id
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // IEnumerable<Expression<Func<TEntity,Object>>>.Aggregate(TAccumulate Seed,Func<TAccumulate,Expression<Func<TEntity,Object>>,TAccumulate> func)

            return query;



        }
    }
}