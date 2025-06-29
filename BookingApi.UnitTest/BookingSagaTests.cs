using BookingApi.Saga;
using BookingApi.Saga.Messages;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApi.SagaUnitTest
{
    public sealed class BookingSagaTests : IAsyncDisposable
    {
        private readonly ServiceProvider _provider;
        private readonly ITestHarness _harness;

        public BookingSagaTests()
        {
            _provider = new ServiceCollection()
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddSagaStateMachine<BookingSaga, BookingSagaData>()
                        .InMemoryRepository();
                })
                .BuildServiceProvider(true);

            _harness = _provider.GetRequiredService<ITestHarness>();
        }

        [Fact]
        public async Task Should_Start_Saga_When_HotelBooked_Event_Received()
        {
            // Arrange
            await _harness.Start();
            var travelerId = NewId.NextGuid();
            var sagaHarness = _harness.GetSagaStateMachineHarness<BookingSaga, BookingSagaData>();

            // Act
            await _harness.Bus.Publish(new HotelBooked
            {
                TravelerId = travelerId,
                HotelName = "Grand Hotel",
                FlightCode = "FL123",
                CarPlateNumber = "ABC123",
                Email = "test@example.com"
            });

            // Assert
            Assert.True(await sagaHarness.Consumed.Any<HotelBooked>());
            Assert.True(await sagaHarness.Created.Any(x => x.CorrelationId == travelerId));

            var saga = sagaHarness.Created.ContainsInState(travelerId, sagaHarness.StateMachine, sagaHarness.StateMachine.FlightBooking);
            Assert.NotNull(saga);
            Assert.True(saga.HotelBooked);
            Assert.Equal("Grand Hotel", saga.HotelName);
            Assert.Equal("FL123", saga.FlightCode);
            Assert.Equal("ABC123", saga.CarPlateNumber);
            Assert.Equal("test@example.com", saga.Email);

            // Verify BookFlight message is published
            Assert.True(await _harness.Published.Any<BookFlight>());

            await _harness.Stop();
        }

        public async ValueTask DisposeAsync()
        {
            await _provider.DisposeAsync();
        }
    }
}
