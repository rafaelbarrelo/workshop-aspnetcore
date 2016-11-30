using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using WorkshopAspNetCore.Models;
using Xunit;

namespace UnitTest.Configuration
{
    [Collection("Base collection")]
    public abstract class BaseIntegrationTest 
    {
        public TestServer Server { get; set; }
        public HttpClient Client { get; set; }
        public DataContext TestDataConext { get; set; }

        protected BaseTestFixture Fixture { get; set; }

        protected BaseIntegrationTest(BaseTestFixture fixture) {
            this.Fixture = fixture;
            this.TestDataConext = fixture.TestDataContext;
            this.Server = fixture.Server;
            this.Client = fixture.Client;

            ClearDb().Wait();
        }

        private async Task ClearDb(){
            var commands = new [] {
                "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'",
                "EXEC sp_MSForEachTable 'DELETE FROM ?'",
                "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'"
            };

            await TestDataConext.Database.OpenConnectionAsync();

            foreach (var command in commands) {
                await TestDataConext.Database.ExecuteSqlCommandAsync(command);
            }

            TestDataConext.Database.CloseConnection();            
        }

    }
}