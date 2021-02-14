using dotnetpostgres.Request;
using dotnetpostgres.Request.Criteria.Parameter;
using dotnetpostgres.Response;

namespace dotnetpostgres.Services.Parameter
{
    public interface IParameterService : ICrudService<Dto.Parameter.Parameter>
    {
        PagedListResponse<Dto.Parameter.Parameter> Search(FilteredPagedListRequest<SearchParameterCriteria> request);
    }
}
