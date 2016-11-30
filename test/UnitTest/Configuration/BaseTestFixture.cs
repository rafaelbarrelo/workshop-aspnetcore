using WorkshopAspNetCore.Models;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using WorkshopAspNetCore;

namespace UnitTest.Configuration
{
    public class BaseTestFixture : IDisposable
    {
        public readonly TestServer Server;
        public readonly HttpClient Client;
        public readonly DataContext TestDataContext;
        private readonly IConfigurationRoot Configuration;

        public BaseTestFixture(){
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var opts = new DbContextOptionsBuilder<DataContext>();
            opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            TestDataContext = new DataContext(opts.Options);
            SetupDatabase();

            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = Server.CreateClient();
        }
        
        private void SetupDatabase(){
            try {
                TestDataContext.Database.EnsureCreated();
                TestDataContext.Database.Migrate();
            }
            catch (Exception){

            }
        }

        void IDisposable.Dispose()
        {
            TestDataContext.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}