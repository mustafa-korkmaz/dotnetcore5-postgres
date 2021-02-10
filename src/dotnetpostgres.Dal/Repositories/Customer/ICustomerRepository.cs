using System;
using System.Collections.Generic;
using dotnetpostgres.Common.Request;
using dotnetpostgres.Common.Request.Criteria.Customer;
using dotnetpostgres.Common.Response;

namespace dotnetpostgres.Dal.Repositories.Customer
{
    public interface ICustomerRepository : IRepository<Entities.Customer>
    {
        PagedListResponse<Entities.Customer> Search(FilteredPagedListRequest<SearchCustomerCriteria> criteria);

        IEnumerable<Entities.Customer> GetAll(Guid userId);
    }
}
