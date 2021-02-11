using System;
using System.Collections.Generic;
using dotnetpostgres.Request;
using dotnetpostgres.Request.Criteria.Customer;
using dotnetpostgres.Response;

namespace dotnetpostgres.Dal.Repositories.Customer
{
    public interface ICustomerRepository : IRepository<Entities.Customer>
    {
        PagedListResponse<Entities.Customer> Search(FilteredPagedListRequest<SearchCustomerCriteria> criteria);

        IEnumerable<Entities.Customer> GetAll(Guid userId);
    }
}
