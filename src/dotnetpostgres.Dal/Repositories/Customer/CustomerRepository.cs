using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Repositories;
using dotnetpostgres.Common;
using dotnetpostgres.Common.Request;
using dotnetpostgres.Common.Request.Criteria.Customer;
using dotnetpostgres.Common.Response;
using Microsoft.EntityFrameworkCore;

namespace dotnetpostgres.Dal.Repositories.Customer
{
    public class CustomerRepository : PostgreSqlDbRepository<Entities.Customer, int>, ICustomerRepository
    {
        public CustomerRepository(Postgres.PostgresDbContext context) : base(context)
        {

        }

        public PagedListResponse<Entities.Customer> Search(FilteredPagedListRequest<SearchCustomerCriteria> request)
        {
            var result = new PagedListResponse<Entities.Customer>();

            var query = Entities.Where(p => p.UserId == request.FilterCriteria.UserId);

            if (!string.IsNullOrEmpty(request.FilterCriteria.Title))
            {
                var titleLikeText = $"%{request.FilterCriteria.Title}%";
                query = query.Where(p => EF.Functions.ILike(p.Title, titleLikeText));
            }

            if (!string.IsNullOrEmpty(request.FilterCriteria.AuthorizedPersonName))
            {
                var personLikeText = $"{request.FilterCriteria.AuthorizedPersonName}%";
                query = query.Where(p => EF.Functions.ILike(p.AuthorizedPersonName, personLikeText));
            }

            if (request.IncludeRecordsTotal)
            {
                result.RecordsTotal = query.Count();
            }

            switch (request.FilterCriteria.SortType)
            {
                case SortType.Ascending:
                    query = query
                        .OrderBy(p => p.Title);
                    break;
                case SortType.Descending:
                    query = query
                        .OrderByDescending(p => p.Title);
                    break;
                default:
                    query = query
                        .OrderByDescending(p => p.Id);
                    break;
            }

            result.Items = query
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToList();

            return result;
        }

        public IEnumerable<Entities.Customer> GetAll(Guid userId)
        {
            var query = Entities.Where(p => p.UserId == userId);

            return query.ToList();
        }
    }
}
