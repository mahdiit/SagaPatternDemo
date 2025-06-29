using BookingApi.Database;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApi.IntegrationTest
{
    public class BaseIntegrationTest
        : IClassFixture<IntegrationTestWebAppFactory>,
            IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly IBus Sender;
        protected readonly BookingDbContext DbContext;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();

            Sender = _scope.ServiceProvider.GetRequiredService<IBus>();

            DbContext = _scope.ServiceProvider.GetRequiredService<BookingDbContext>();
        }

        protected virtual void Dispose(bool disposing)
        {
            _scope?.Dispose();
            DbContext?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
