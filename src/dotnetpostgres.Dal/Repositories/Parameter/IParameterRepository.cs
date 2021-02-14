using dotnetpostgres.Request;
using dotnetpostgres.Request.Criteria.Parameter;
using dotnetpostgres.Response;

namespace dotnetpostgres.Dal.Repositories.Parameter
{
    public interface IParameterRepository : IRepository<Entities.Parameter>
    {
        PagedListResponse<Entities.Parameter> Search(FilteredPagedListRequest<SearchParameterCriteria> request);
    }
}
