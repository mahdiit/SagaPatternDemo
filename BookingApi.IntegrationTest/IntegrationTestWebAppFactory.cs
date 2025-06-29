using BookingApi.Database;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace BookingApi.IntegrationTest
{
    public class IntegrationTestWebAppFactory
        : WebApplicationFactory<Program>,
            IAsyncLifetime
    {
        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:4")
            .WithPortBinding(5672, true)
            .WithUsername("guest")
            .WithPassword("guest")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();

        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("MSSQL_PID", "Express")
            .WithPassword("123@Admin")
            .WithPortBinding(1433, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .Build();

        public async Task InitializeAsync()
        {
            await _rabbitMqContainer.StartAsync();
            await _msSqlContainer.StartAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var newConfig = new Dictionary<string, string>
                {
                    ["ConnectionStrings:BookingDb"] = GetSqlConnection(),
                    ["ConnectionStrings:RabbitMq"] = GetRabbitmqConnection()
                };

                configBuilder.AddInMemoryCollection(newConfig);
            });

            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
                db.Database.Migrate();
            });
        }

        private string GetSqlConnection()
        {
            var port = _msSqlContainer.GetMappedPublicPort(1433);
            return $"Database=BookingDb;Application Name=BookingApi;Integrated Security=false;Server={_msSqlContainer.Hostname},{port};User ID=sa;Password=123@Admin;TrustServerCertificate=True;";
        }

        private string GetRabbitmqConnection()
        {
            return $"rabbitmq://{_rabbitMqContainer.Hostname}:{_rabbitMqContainer.GetMappedPublicPort(5672)}";
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _rabbitMqContainer.StopAsync();
            await _msSqlContainer.StopAsync();
        }
    }
}
