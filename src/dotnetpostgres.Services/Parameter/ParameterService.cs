using System.Collections.Generic;
using AutoMapper;
using dotnetpostgres.Dal;
using dotnetpostgres.Dal.Repositories.Parameter;
using dotnetpostgres.Request;
using dotnetpostgres.Request.Criteria.Parameter;
using dotnetpostgres.Response;
using Microsoft.Extensions.Logging;

namespace dotnetpostgres.Services.Parameter
{
    public class ParameterService : CrudService<IParameterRepository, Dal.Entities.Parameter, Dto.Parameter.Parameter>, IParameterService
    {
        public ParameterService(IUnitOfWork uow, ILogger<ParameterService> logger, IMapper mapper)
        : base(uow, logger, mapper)
        {
            ValidateEntityOwner = true;
        }

        public PagedListResponse<Dto.Parameter.Parameter> Search(FilteredPagedListRequest<SearchParameterCriteria> request)
        {
            var resp = Repository.Search(request);

            var parameters = Mapper.Map<IEnumerable<Dal.Entities.Parameter>, IEnumerable<Dto.Parameter.Parameter>>(resp.Items);

            return new PagedListResponse<Dto.Parameter.Parameter>
            {
                Items = parameters,
                RecordsTotal = resp.RecordsTotal
            };
        }
    }
}
