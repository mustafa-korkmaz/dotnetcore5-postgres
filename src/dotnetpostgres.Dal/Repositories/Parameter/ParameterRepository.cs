using System.Linq;
using dotnetpostgres.Dal.Databases.Postgres;
using dotnetpostgres.Request;
using dotnetpostgres.Request.Criteria.Parameter;
using dotnetpostgres.Response;
using Microsoft.EntityFrameworkCore;

namespace dotnetpostgres.Dal.Repositories.Parameter
{
    public class ParameterRepository : PostgreSqlDbRepository<Entities.Parameter, int>, IParameterRepository
    {
        public ParameterRepository(PostgresDbContext context) : base(context)
        {

        }

        public PagedListResponse<Entities.Parameter> Search(FilteredPagedListRequest<SearchParameterCriteria> request)
        {
            var result = new PagedListResponse<Entities.Parameter>();

            var query = Entities.Where(p => p.IsDeleted == false);

            if (!string.IsNullOrEmpty(request.FilterCriteria.Name))
            {
                var nameLikeText = string.Format("%{0}%", request.FilterCriteria.Name);
                query = query.Where(p => EF.Functions.ILike(p.Value, nameLikeText));
            }

            if (request.IncludeRecordsTotal)
            {
                result.RecordsTotal = query.Count();
            }

            result.Items = query
                .OrderBy(p => p.Order)
                .ThenBy(p => p.Value)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToList();

            return result;
        }
    }
}
