using Core.Entities;
using Core.specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Data
{
    class SpecificationEvaluator<TEntity> where TEntity:BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec)
        {
            var query = inputQuery;
            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

      
            #region sorting
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            #endregion

            #region Paging
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            #endregion
            query = spec.Includes.Aggregate(query,(current,include) => current.Include(include));
            return query;
        }
    }
}
