using System;
using System.Collections.Generic;
using System.Net.Http;
using dotnetpostgres.Services.Customer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace dotnetpostgres.tests
{
    /// <summary>
    /// CachingMethodName_Should_ExpectedBehavior_When_StateUnderTest
    /// </summary>
    public class CachingTests
    {
        private readonly ICustomerService _customerService;
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public CachingTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();

            _customerService = _server.Services.GetService<ICustomerService>();
        }

        [Fact]
        public void CacheKey_Should_Be_Fetched_When_MethodIsGiven()
        {
            //arrange
            var c = this;

            Action action = c.CacheKey_Should_Be_Fetched_When_MethodIsGiven;
            Func<int, string> func = c.MyTestMethod;

            //act
            var actionCacheKey = Utility.GetMethodResultCacheKey(action, new List<string> { "test1", "test2" });
            var funcCacheKey = Utility.GetMethodResultCacheKey(func, new List<object> { 1000 });

            //assert
            Assert.Equal("dotnetpostgres.tests.CachingTests.CacheKey_Should_Be_Fetched_When_MethodIsGiven(test1, test2)", actionCacheKey);
            Assert.Equal("dotnetpostgres.tests.CachingTests.MyTestMethod(1000)", funcCacheKey);
        }

        private string MyTestMethod(int a)
        {
            return a.ToString();
        }
            
    }
}
