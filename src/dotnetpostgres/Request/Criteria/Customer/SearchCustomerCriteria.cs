using System;

namespace dotnetpostgres.Request.Criteria.Customer
{
    public class SearchCustomerCriteria
    {
        public string Title { get; set; }

        public string AuthorizedPersonName { get; set; }

        public SortType SortType { get; set; }
    }
}
