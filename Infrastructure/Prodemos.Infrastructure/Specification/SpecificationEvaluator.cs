using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Specification;

namespace Prodemos.Infrastructure.Specification;
public class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        if (specification.Criteria is not null)
        {
            inputQuery = inputQuery.Where(specification.Criteria);
        }

        if (specification.OrderBy is not null)
        {
            inputQuery = inputQuery.OrderBy(specification.OrderBy);
        }

        if (specification.OrderByDescending is not null)
        {
            inputQuery = inputQuery.OrderBy(specification.OrderByDescending);
        }

        if (specification.IsPagingEnable)
        {
            inputQuery = inputQuery.Skip(specification.Skip).Take(specification.Take);
        }

        inputQuery = specification.Includes!.Aggregate(inputQuery,
                                                        (current, include) => current.Include(include))
                                            .AsSplitQuery()
                                            .AsNoTracking();

        return inputQuery;
    }
}
