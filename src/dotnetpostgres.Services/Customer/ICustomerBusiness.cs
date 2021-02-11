using dotnetpostgres.Request;
using dotnetpostgres.Request.Criteria.Customer;
using dotnetpostgres.Response;

namespace dotnetpostgres.Services.Customer
{
    public interface ICustomerService : ICrudService<Dto.Customer.Customer>
    {
        PagedListResponse<Dto.Customer.Customer> Search(FilteredPagedListRequest<SearchCustomerCriteria> request);
    }
}
