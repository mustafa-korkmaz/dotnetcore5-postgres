using System.Collections.Generic;
using AutoMapper;
using dotnetpostgres.Dal;
using dotnetpostgres.Dal.Repositories.Customer;
using dotnetpostgres.Request;
using dotnetpostgres.Request.Criteria.Customer;
using dotnetpostgres.Response;
using Microsoft.Extensions.Logging;

namespace dotnetpostgres.Services.Customer
{
    public class CustomerService : CrudService<ICustomerRepository, Dal.Entities.Customer, Dto.Customer.Customer>, ICustomerService
    {
        public CustomerService(IUnitOfWork uow, ILogger<CustomerService> logger, IMapper mapper)
        : base(uow, logger, mapper)
        {
            ValidateEntityOwner = true;
        }

        public PagedListResponse<Dto.Customer.Customer> Search(FilteredPagedListRequest<SearchCustomerCriteria> request)
        {
            var resp = Repository.Search(request);

            var customers = Mapper.Map<IEnumerable<Dal.Entities.Customer>, IEnumerable<Dto.Customer.Customer>>(resp.Items);

            return new PagedListResponse<Dto.Customer.Customer>
            {
                Items = customers,
                RecordsTotal = resp.RecordsTotal
            };
        }
    }
}
