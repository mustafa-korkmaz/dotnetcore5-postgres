using System;
using System.Collections.Generic;
using System.Net.Http;
using dotnetpostgres.Dto.Customer;
using dotnetpostgres.Services.Customer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace dotnetpostgres.tests
{
    public class CustomerServiceTests
    {
        private readonly ICustomerService _customerService;
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public CustomerServiceTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();

            _customerService = _server.Services.GetService<ICustomerService>();
        }

        [Fact]
        public void Customer_Should_Be_Found_When_ProperIsGiven()
        {
            //arrange
            CreateCustomersInDatabase();

            //act
            var response1 = _customerService.Get(1000);
            var response2 = _customerService.Get(1001);
            var response3 = _customerService.Get(9002);

            //assert
            Assert.NotNull(response1);
            Assert.NotNull(response2);
            Assert.NotNull(response3);

            Assert.True(response1.Type == ResponseType.Success);
            Assert.True(response2.Type == ResponseType.Success);
            Assert.True(response3.Type == ResponseType.RecordNotFound);
        }

        private void CreateCustomersInDatabase()
        {
            var customers = new List<Customer>();

            for (int i = 0; i < 100; i++)
            {
                var title = "Customer " + i;
                Random rand = new Random();

                customers.Add(new Customer
                {
                    Id = 1000 + i,
                    Title = title,
                    AuthorizedPersonName = title
                });
            }

            _customerService.AddRange(customers.ToArray());
        }
    }
}
